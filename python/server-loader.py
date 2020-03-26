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

# pip3 install fhir.resources  ### TODO: remove if not needed
from fhir.resources.group import Group
from fhir.resources.location import Location

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

        if not os.path.isdir(folder):
            raise Error(f'{folder} is not a directory.')

        # Find all the contained data files in the root folder.
        data_files = []
        for dirname, _, files in os.walk(folder):
            for file in files:
                filename = os.path.join(dirname, file)
                if DataSource.is_data_file(filename):
                    data_files.append(filename)

        self._data_files = data_files

    def __iter__(self):
        return self._data_files.__iter__()


class Server:
    """A FHIR Server to receive data."""

    # This tag is applied to each loaded bundle.
    _resource_tags = [
        {
            'system': 'https://github.com/microsoft-healthcare-madison/learning-spike-erp',
            'code': 'sample-data',
        },
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
        self._ip = socket.gethostbyname(url.netloc)  # Confirm DNS resolution.
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

    def get(self, relative_url):
        response = requests.get(self.url + relative_url, headers=self.headers)
        return json.loads(response.text)

    def post(self, relative_url, json_body):
        print("POSTing", json_body)  # XXX
        response = requests.post(self.url + relative_url, json=json_body, headers=self.headers)
        return json.loads(response.text)

    def get_all_tagged_resource_entries(self):
        tag = self._resource_tags[0]
        tag_query = '|'.join([tag['system'], tag['code']])
        paged_query = f'?_count=1000&_tag={tag_query}'

        resource = self.get(paged_query)
        while resource.get('entry'):
            print("RESOURCE", resource)  # XXX
            for entry in resource.get('entry'):
                yield entry
            resource = self.get(paged_query)
            resource = {}

    def delete_all_tagged_resources(self):
        tag = self._resource_tags[0]
        tag_query = "|".join([tag['system'], tag['code']])

        def get_next_deletes():
            return self.get(f'?_count=1000&_tag={tag_query}')

        to_delete = get_next_deletes()
        while to_delete.get('entry', []):
            print("Deleting a batch of %s"%len(to_delete['entry']))
            delete_result = self.post('', {
                'resourceType': 'Bundle',
                'type': 'batch',
                'entry': [{
                    'request': {
                        'method': 'DELETE',
                        'url': e['resource']['resourceType'] + '/' + e['resource']['id']
                    },
                    'resource': e['resource']
                } for e in to_delete['entry']]
            })
            print("deleted", delete_result)
            to_delete = get_next_deletes()

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
        e['resource'].setdefault('meta', {})['tag'] = cls._resource_tags
        return e

    def annotate_bundle_entries(self, bundle):
        print('annotating bundle:')  # XXX
        pprint.pprint(bundle['entry'])
        for e in bundle['entry']:
            e['request'] = {
                'method': 'PUT',
                'url': '/'.join([
                    e['resource']['resourceType'],
                    e['resource']['id']
                ]),
            }
            e['resource'].setdefault('meta', {})['tag'] = self._resource_tags
        return bundle

    def receive(self, filename):

        # http://hl7.org/fhir/bundle-definitions.html#Bundle.entry.request has details 
        # * method, url are required
        # method = "PUT"
        # URL = "Organization" for example -- more specifically, `${entry.resourceType}`

        with open(filename, 'rb') as fd:
            resource = json.load(fd)
        if resource.get('resourceType') == 'Bundle':
            self.annotate_bundle_entries(resource)
        headers = {
            'accept': 'application/fhir+json',
            'content-type': 'application/fhir+json'
        }
        
        # https://prototype-erp-fhir.azurewebsites.net/Organization/Org-1067?_format=json
        # http://prototype-erp-fhir.azurewebsites.net/Organization?_tag=sample-data&_format=json
        # http://prototype-erp-fhir.azurewebsites.net/?_tag=sample-data&_format=json
        #  :-( not supported curl -X DELETE 'http://prototype-erp-fhir.azurewebsites.net/?_tag=sample-data&_format=json'
        
        r = requests.post(self._url.geturl(), json=resource, headers=headers)
        if r.status_code != 200:
            print('failed to send bundle to server', r.reason)
            return
        print(json.loads(r.content))
        for entry in json.loads(r.content)['entry']:
            if entry['response']['status'] != '200':
                print(f'Entry failed to load: {entry}')


@click.command()
@click.option('--server-url', default=TEST_SERVER, help='FHIR server URL.')
@click.option('--delete-all', default=False, is_flag=True, help='Set this to delete all tagged resources from the sevrver')
@click.option('--files', default='.', help='Path to a directory containing .json files to send to the server.')
def main(server_url, delete_all, files):
    server = Server(server_url)
    if delete_all:
#        print(list(server.get_all_tagged_resource_entries()))  # XXX
        server.delete_all_tagged_resources()

    for filename in DataSource(files):
        server.receive(filename)


if __name__ == '__main__':
    main()
