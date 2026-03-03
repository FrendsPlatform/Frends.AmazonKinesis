# Frends.AmazonKinesis.PutRecords

Frends Task for sending data records to an Amazon Kinesis Data Stream.

[![PutRecords_build](https://github.com/FrendsPlatform/Frends.AmazonKinesis/actions/workflows/PutRecords_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.AmazonKinesis/actions/workflows/PutRecords_test_on_main.yml)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.AmazonKinesis/Frends.AmazonKinesis.PutRecords|main)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

## Installing

You can install the Task via Frends UI Task View.

## Building

### Clone a copy of the repository

`git clone https://github.com/FrendsPlatform/Frends.AmazonKinesis.git`

### Build the project

`dotnet build`

### Run tests

Run the tests

`dotnet test`

### Create a NuGet package

`dotnet pack --configuration Release`

### StyleCop.Analyzers Version
This project uses StyleCop.Analyzers 1.2.0-beta.556, as recommended by the author, to get the latest fixes and improvements not available in the last stable release.

### Third-party licenses
AWSSDK.Kinesis used for AWS Kinesis client implementation is licensed under the Apache-2.0 license.
The full license text and source code can be found at https://github.com/aws/aws-sdk-net.