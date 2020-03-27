#!/usr/local/bin/python3
"""A script to load FHIR resources into an open FHIR server.

TODO:
    [ ] convert this to use setuptools (so it works in windows)
"""

import click  # pip3 install click
import copy
import json
import os
import pprint  # XXX
import requests
import socket
import sys
from urllib.parse import urlparse

RESOURCE_TAGS = [{
    'system': 'https://github.com/microsoft-healthcare-madison/learning-spike-erp',  # nopep8
    'code': 'sample-data',
}]

RESOURCS_CREATED = ['MeasureReport', 'Group', 'Location', 'Organization']

TEST_SERVER = 'https://prototype-erp-fhir.azurewebsites.net/'


class Error(Exception):
    pass


class DataSource:
    """A folder of files containing data to send to a server."""

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
        return extension in ['json', 'xml']

    def __init__(self, folder):
        self._data_files = []

        if folder:
            if not os.path.isdir(folder):
                raise Error(f'{folder} is not a directory.')

            # Find all the contained data files in the root folder.
            data_files = []
            for dirname, _, files in os.walk(folder):
                for file in sorted(files, key=lambda f: 4 if "-measureReport" in f else 3 if "-group" in f else 2 if "-bed" in f else 1):
                    filename = os.path.join(dirname, file)
                    if DataSource.is_data_file(filename):
                        data_files.append(filename)

            self._data_files = data_files
            print("Data files", data_files)

    def __iter__(self):
        return self._data_files.__iter__()


class Server:
    """A FHIR Server to receive data."""

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
        print("got", url, response.status_code)
        return json.loads(response.text)

    def post(self, relative_url, json_body):
        print("POSTing to", relative_url)  # XXX
        response = requests.post(
            self.url + relative_url,
            json=json_body,
            headers=self.headers
        )
        return json.loads(response.text)

    def get_all_tagged_resource_entries(self):
        tag = RESOURCE_TAGS[0]
        tag_query = '|'.join([tag['system'], tag['code']])
        paged_query = f'?_count=1000&_tag={tag_query}'

        resource = self.get(paged_query)
        while resource.get('entry'):
            print("RESOURCE", resource)  # XXX
            for entry in resource.get('entry'):
                yield entry
            resource = self.get(paged_query)

    # The order of resource_types matter because HAPI won't allow dangling links by default
    def delete_all_tagged_resources(self, resource_types = RESOURCS_CREATED):
        tag = RESOURCE_TAGS[0]
        tag_query = "|".join([tag['system'], tag['code']])

        def search_for_deletes_deletes(resource_type):
            sort = '&_sort=partof' if resource_type == 'Location' else ''
            next_deletes = self.get(f'/{resource_type}?_count=1000&_tag={tag_query}{sort}')
            print("Num next deltes", len(next_deletes))
            return next_deletes

        for resource_type in resource_types:
            to_delete = search_for_deletes_deletes(resource_type)
            while to_delete:
                print(f'Deleting a batch of { len(to_delete.get("entry", [])) }')
                delete_result = self.post('', {
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
                    } for e in to_delete.get('entry', [])]
                })
                print("deleted", delete_result)
                bundle_next_link = [link['url'] for link in to_delete.get('link', []) if link.get('relation', '') == 'next']
                to_delete = self.get(bundle_next_link[0]) if bundle_next_link else []
            print("Finished ", resource_type)

    @classmethod
    def annotate_bundle_entry(cls, entry):  # XXX keep???
        e = copy.deepcopy(entry)
        e['request'] = {
            'method': 'PUT',
            'url': '/'.join([
                e['resource']['resourceType'],
                e['resource']['id']
            ]),
        }
        e['resource'].setdefault('meta', {})['tag'] = RESOURCE_TAGS
        return e

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
        if resource.get('resourceType') == 'Bundle':
            self.annotate_bundle(resource)

        r = requests.post(
            self._url.geturl(), json=resource, headers=self.headers
        )
        if r.status_code != 200:
            print('failed to send bundle to server', r.reason)
            pprint.pprint(r.__dict__)  # XXX
            pprint.pprint(r.raw.__dict__)  # XXX
            return
        print(json.loads(r.content))


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
