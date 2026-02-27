using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Frends.AmazonKinesis.PutRecords.Definitions;
using NUnit.Framework;

namespace Frends.AmazonKinesis.PutRecords.Tests;

[TestFixture]
public class FunctionalTests : TestBase
{
    [Test]
    public async Task PutRecords_SingleRecord_Success()
    {
        var input = DefaultInput();
        input.Records = new List<LogEntry>
        {
            new() { Data = "test message", PartitionKey = "partition-1" },
        };

        var result = await AmazonKinesis.PutRecords(input, DefaultConnection(), DefaultOptions(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.FailedRecordCount, Is.EqualTo(0));
        Assert.That(result.Entries, Has.Count.EqualTo(1));
        Assert.That(result.Entries[0].IsSuccessful, Is.True);
        Assert.That(result.Entries[0].ShardId, Is.Not.Null);
        Assert.That(result.Entries[0].SequenceNumber, Is.Not.Null);
    }

    [Test]
    public async Task PutRecords_MultipleRecords_Success()
    {
        var input = DefaultInput();
        input.Records = new List<LogEntry>
        {
            new() { Data = "message 1", PartitionKey = "partition-1" },
            new() { Data = "message 2", PartitionKey = "partition-2" },
            new() { Data = "{ \"status\": 200, \"message\": \"ok\" }", PartitionKey = "partition-3" },
        };

        var result = await AmazonKinesis.PutRecords(input, DefaultConnection(), DefaultOptions(), CancellationToken.None);

        Assert.That(result.Success, Is.True);
        Assert.That(result.FailedRecordCount, Is.EqualTo(0));
        Assert.That(result.Entries, Has.Count.EqualTo(3));
        Assert.That(result.Entries.All(e => e.IsSuccessful), Is.True);
    }

    [Test]
    public void PutRecords_InvalidCredentials_ThrowsException()
    {
        var connection = new Connection
        {
            AwsAccessKeyId = "invalid-key",
            AwsSecretAccessKey = "invalid-secret",
            Region = Region.EuCentral1,
        };
        var input = DefaultInput();
        input.Records = new List<LogEntry>
        {
            new() { Data = "test", PartitionKey = "partition-1" },
        };

        Assert.ThrowsAsync<Exception>(() =>
            AmazonKinesis.PutRecords(input, connection, DefaultOptions(), CancellationToken.None));
    }
}
