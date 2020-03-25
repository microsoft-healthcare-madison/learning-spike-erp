#!/usr/local/bin/python3
"""A script to load FHIR resources into an open FHIR server.

TODO:
    [ ] convert this to use setuptools (so it works in windows)
"""

import click  # pip3 install click
import json
import os
import requests
import socket
import sys
from urllib.parse import urlparse

# pip3 install fhir.resources
from fhir.resources.group import Group
from fhir.resources.location import Location

TEST_SERVER = 'https://prototype-erp-fhir.azurewebsites.net/'


class Error(Exception):
    pass


class DataSource:
    """A source of data to send to a server."""

    @classmethod
    def is_data_file(cls, filename):
        if not os.path.isfile(filename):
            raise Error(f'File: {filename} does not exist.')

        fd = open(filename, 'rb')
        head = fd.read(32)
        if b'{' not in head:
            return False

        # Skip to the last 32 bytes of the file.
        fd.seek(0 - min(fd.seek(0, 2), 32), 2)
        return b'}' in fd.read()

    def __init__(self, folder):
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

    def __init__(self, server_url):
        url = urlparse(server_url)
        if url.scheme == 'http' and not url.netloc.lower() == 'localhost':
            raise Error('Refusing to send data over http.')

        self._ip = socket.gethostbyname(url.netloc)
        self._metadata = requests.get(server_url + '/metadata')
        self._url = url

        if self._metadata.status_code != 200:
            raise Error(f'Failed to read {server_url}/metadata')

    def receive(self, filename):
        # TODO: use the requests module to post to server.
        # TODO: change the mime-type to fhir+json
        pass



@click.command()
@click.option('--server-url', default=TEST_SERVER, help='FHIR server URL.')
@click.option('--files', help='Source files to send to the server.')
def main(server_url, files):
    server = Server(server_url)
    for filename in DataSource(files):
        server.receive(filename)


if __name__ == '__main__':
    main()
