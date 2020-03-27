#!/usr/local/bin/python3
"""A script to load FHIR resources into an open FHIR server.

TODO:
    [ ] convert this to use setuptools (so it works in windows)
"""

import copy
import json
import os
import pprint  # XXX
import re
import socket
import sys
from urllib.parse import urlparse

import click  # pip3 install click
import requests  # pip3 install requests

RESOURCE_TAGS = [{
    'system': 'https://github.com/microsoft-healthcare-madison/learning-spike-erp',  # nopep8
    'code': 'sample-data',  # TODO: make this code a flag called --tag-code
}]

TEST_SERVER = 'https://prototype-erp-fhir.azurewebsites.net/'


class Error(Exception):
    pass


class DataSource:
    """A folder of files containing data to send to a server."""

    _resource_file_matcher = re.compile(r'^Org-([0-9]+)(-.+)?\.json$')
    _resource_type_load_priorities = {
        '-measureReports': 4,
        '-groups': 3,
        '-beds': 2,
        None: 1,
    }

    @classmethod
    def has_json_contents(cls, filename):
        """Inspects a file to see if it looks like JSON."""

        if not os.path.isfile(filename):
            raise Error(f'File: {filename} does not exist.')

        fd = open(filename, 'rb')
        head = fd.read(32)
        if b'{' not in head:
            return False

        # Skip to the last 32 bytes of the file.
        fd.seek(0 - min(fd.seek(0, 2), 32), 2)
        return b'}' in fd.read()

    @classmethod
    def is_data_file(cls, filename):
        """Returns True when a file exists and ends with a known extension."""

        if not os.path.isfile(filename):
            raise Error(f'File: {filename} does not exist.')
        extension = filename.lower().rsplit('.', 1)[-1]
        return extension in ['json']

    @classmethod
    def get_resource_load_priority(cls, filename):
        """Returns the load priority based on resource type and org id."""
        match = cls._resource_file_matcher.match(filename)
        if not match:
            return (0, 0)
        org, resource_type = match.groups()
        priority = cls._resource_type_load_priorities.get(resource_type, 0)
        return (priority, int(org))

    @classmethod
    def get_data_files(cls, folder):
        for dirname, _, files in os.walk(folder):
            for file in sorted(files, key=cls.get_resource_load_priority):
                filename = os.path.join(dirname, file)
                if DataSource.is_data_file(filename):
                    yield filename

    def __init__(self, folder):
        self._data_files = []

        if folder:
            if not os.path.isdir(folder):
                raise Error(f'{folder} is not a directory.')

            self._data_files = list(self.get_data_files(folder))
            print("Data files", self._data_files)  # XXX

    def __iter__(self):
        return self._data_files.__iter__()


class Server:
    """A FHIR Server to receive data."""

    # The order of resource types matters because HAPI won't allow dangling
    # links by default.
    _deletion_order = ['MeasureReport', 'Group', 'Location', 'Organization']

    _headers = {
        'accept': 'application/fhir+json',
        'content-type': 'application/fhir+json'
    }

    def __init__(self, server_url):
        """Initialize a Server object."""

        url = urlparse(server_url)
        if url.scheme == 'http' and not url.netloc.lower() == 'localhost':
            raise Error('Refusing to send data over http.')

        self._capabilities = None
        hostname = url.netloc.split(':')[0]
        self._ip = socket.gethostbyname(hostname)  # Confirm DNS resolution.
        self._metadata = None
        self._url = url

        self._metadata = requests.get(server_url + '/metadata')
        if self._metadata.status_code != 200:
            raise Error(f'Failed to read {server_url}/metadata')
        self._capabilities = self._metadata.json()

    @property
    def headers(self):
        return self._headers

    @property
    def url(self):
        return self._url.geturl()

    def get(self, url):
        full_url = url if url.startswith('http') else self.url + url
        response = requests.get(full_url, headers=self.headers)
        return json.loads(response.text)

    def post(self, relative_url, json_body):
        response = requests.post(
            self.url + relative_url,
            json=json_body,
            headers=self.headers
        )
        return json.loads(response.text)

    def delete_all_tagged_resources(self):
        tag = RESOURCE_TAGS[0]
        tag_query = '|'.join([tag['system'], tag['code']])

        def search_for_deletes(resource_type):
            sort = '&_sort=partof' if resource_type == 'Location' else ''
            query = f'/{resource_type}?_count=1000&_tag={tag_query}{sort}'
            return self.get(query)

        for resource_type in self._deletion_order:
            print(f'DELETING RESOURCE: {resource_type}')
            to_delete = search_for_deletes(resource_type)
            while to_delete:
                entries = to_delete.get('entry', [])
                self.post('', {
                    'resourceType': 'Bundle',
                    'type': 'batch',
                    'entry': [{
                        'request': {
                            'method': 'DELETE',
                            'url': '/'.join([
                                e['resource']['resourceType'],
                                e['resource']['id'],
                            ])
                        },
                    } for e in entries]
                })
                more = [x for x in to_delete['link'] if x['relation'] == 'next']  # nopep8
                if not more:
                    break
                print('  ..')
                to_delete = self.get(more[0]['url'])
            print(f'DELETED RESOURCES: {resource_type}')

    def annotate_bundle_entries(self, bundle):
        for e in bundle['entry']:
            e['request'] = {
                'method': 'PUT',
                'url': '/'.join([
                    e['resource']['resourceType'],
                    e['resource']['id']
                ]),
            }
            e['resource'].setdefault('meta', {})['tag'] = RESOURCE_TAGS

    def annotate_bundle(self, bundle):
        self.annotate_bundle_entries(bundle)
        bundle['type'] = 'batch'

    def receive(self, filename):
        with open(filename, 'rb') as fd:
            print(f'LOADING FILE: {filename}...')  # XXX
            resource = json.load(fd)
        if resource['resourceType'] == 'Bundle':
            self.annotate_bundle(resource)

        r = requests.post(
            self._url.geturl(), json=resource, headers=self.headers
        )
        if r.status_code != 200:
            print('failed to send bundle to server', r.reason)
            pprint.pprint(r.__dict__)  # XXX
            pprint.pprint(r.raw.__dict__)  # XXX
            return
#        print(json.loads(r.content))
        for entry in json.loads(r.content)['entry']:
            if entry['response']['status'] not in ['200', '201']:
                print(f'Entry failed to load: {entry}')


@click.command()
@click.option(
    '--server-url', default=TEST_SERVER, help='FHIR server URL.'
)
@click.option(
    '--delete-all', default=False, is_flag=True,
    help='Set this to delete all tagged resources from the sevrver'
)
@click.option(
    '--files',
    help='Path to a directory containing .json files to send to the server.'
)
def main(server_url, delete_all, files):
    server = Server(server_url)
    if delete_all:
        # TODO: print out the number of deleted resources.
        server.delete_all_tagged_resources()

    for filename in DataSource(files):
        server.receive(filename)


if __name__ == '__main__':
    main()  # pylint: disable=no-value-for-parameter
