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

LOADABLE_TYPES = [
    'Group',  # TODO: Decide whether to keep this capability or not.
    'Location',
    'Measure',
    'MeasureReport',
    'Organization',
]

RESOURCE_TAGS = [{
    'system': 'https://github.com/microsoft-healthcare-madison/learning-spike-erp',  # nopep8
    'code': 'sample-data',
}]

TEST_SERVER = 'https://prototype-erp-fhir.azurewebsites.net/'


class Error(Exception):
    pass


class FileProperties:
    """A simple struct to for loadable file type features."""

    load_priority = 0
    contained_resource_types = set()

    def __init__(self, load_priority, resource_types):
        self.load_priority = load_priority
        self.contained_resource_types = set(resource_types)


class DataSource:
    """A folder of files containing data to send to a server."""

    # Examples: ./t0/Org-1234.json, ./data/t1/Org-123-beds.json, Measures.json.
    _resource_file_matcher = re.compile(r'^(.*/)?(X([0-9]+)-?)?(.+)?\.json$')
    _resource_file_properties = {
        'measureReports': FileProperties(7, ['MeasureReport']),
        'Measures': FileProperties(6, ['Measure']),
        'QuestionnaireResponses': FileProperties(5, ['QuestionnaireResponse']),
        'Questionnaires': FileProperties(4, ['Questionnaire']),
        'groups': FileProperties(3, ['Group']),
        'beds': FileProperties(2, ['Location']),
        None: FileProperties(1, ['Organization', 'Location']),  # Org files.
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
    def contains_requested_resources(cls, filename, requested_types):
        """Returns True when the filename should contain the requested types.

        Param:
            filename: str, a filename which is expected to exist.
            requested_types: list of str, the resource types requested to load.
        """
        match = cls._resource_file_matcher.match(filename)
        if not match:
            print(f'Warning, unknown file type: {filename}, skipping...')
            return False
        file_type = match.groups()[-1]
        if file_type:
            file_type = file_type.split('-')[0]  # trim off -CDC or -FEMA.
        available_types = cls._resource_file_properties.get(
            file_type
        ).contained_resource_types
        return available_types.issubset(requested_types)

    @classmethod
    def get_resource_load_priority(cls, filename):
        """Returns the load priority based on resource type and org id."""

        match = cls._resource_file_matcher.match(filename)
        if not match:
            return ('', 0, 0)
        folder, _, org, file_type = match.groups()
        if file_type:
            file_type = file_type.split('-')[0]  # trim off -CDC or -FEMA.

        resource_file_type = cls._resource_file_properties.get(file_type)
        if not resource_file_type:
            print(f'Unknown file type: {file_type}')

        return (folder or '', resource_file_type.load_priority, int(org or 0))

    @classmethod
    def get_data_files(cls, folder, load_types):
        resource_types = set(load_types)
        for dirname, dirs, files in os.walk(folder):
            dirs.sort()  # Needed side-effect: sorts timestamp dir traversal.
            files = [os.path.join(dirname, x) for x in files]
            for file in sorted(files, key=cls.get_resource_load_priority):
                if cls.is_data_file(file):
                    if cls.contains_requested_resources(file, resource_types):
                        yield file

    def __init__(self, folder, load_types):
        self._data_files = []

        if folder:
            if not os.path.isdir(folder):
                raise Error(f'{folder} is not a directory.')

            self._data_files = list(self.get_data_files(folder, load_types))

    def __iter__(self):
        return self._data_files.__iter__()


class Server:
    """A FHIR Server to receive data."""

    # The order of resource types matters because HAPI won't allow dangling
    # links by default.
    _deletion_order = [
        'MeasureReport', 'Measure', 'Group', 'Location', 'Organization'
    ]

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

        # TODO: only delete the resources selected to be loaded?
        for resource_type in self._deletion_order:
            print(f'DELETING RESOURCE: {resource_type}')
            to_delete = search_for_deletes(resource_type)
            while to_delete:
                entries = to_delete.get('entry', [])
#                print(entries)  # XXX
#                print(entries[-1:])  # XXX
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

    def receive(self, filename):
        with open(filename, 'rb') as fd:
            resource = json.load(fd)
            self.annotate_bundle_entries(resource)
            resource['type'] = 'batch'
            return requests.post(
                self._url.geturl(), json=resource, headers=self.headers
            )


def load_file(server, filename):
    print(f'LOADING FILE: {filename}...')
    r = server.receive(filename)
    success = ['200', '201', '200 OK', '201 Created']
    if r.status_code == 200:
        for entry in json.loads(r.content)['entry']:
            if entry['response']['status'] not in success:
                print(f'Entry failed to load: {entry}')
    else:
        print('failed to send bundle to server', r.reason)
        pprint.pprint(r.__dict__)  # XXX
        pprint.pprint(r.raw.__dict__)  # XXX


@click.command()
@click.option(
    '--server-url', '-s', default=TEST_SERVER, help='FHIR server URL.'
)
@click.option(
    '--delete-all', '-D', default=False, is_flag=True,
    help='Set this to delete all tagged resources from the sevrver'
)
@click.option(
    '--load', '-l', multiple=True, default=LOADABLE_TYPES,
    type=click.Choice(LOADABLE_TYPES),
    help='Load resources of this type (flag can be used more than once).'
)
@click.option(
    '--files', '-f',
    help='Path to a directory containing .json files to send to the server.'
)
@click.option(
    '--tag-code', '-t', default=RESOURCE_TAGS[0]['code'],
    help='A tag code to apply to all loaded resources.',
)
def main(server_url, delete_all, load, files, tag_code):
    global RESOURCE_TAGS
    RESOURCE_TAGS[0]['code'] = tag_code

    data_files = DataSource(files, load)

    # TODO: wrap this in a try block, saving any failed files somewhere.
    # TODO: parallelize this so multiple files can be loaded at once.
    server = Server(server_url)
    if delete_all:
        # TODO: print out the number of deleted resources.
        server.delete_all_tagged_resources()

    for filename in data_files:
        load_file(server, filename)


if __name__ == '__main__':
    main()  # pylint: disable=no-value-for-parameter
