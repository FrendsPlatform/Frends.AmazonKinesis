using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Frends.AmazonKinesis.PutRecords.Definitions;
using Moq;
using NUnit.Framework;

namespace Frends.AmazonKinesis.PutRecords.Tests;

[TestFixture]
public class UnitTests
{
    private Mock<IAmazonKinesis> _kinesisClientMock;

    [SetUp]
    public void SetUp()
    {
        _kinesisClientMock = new Mock<IAmazonKinesis>();
        AmazonKinesis.KinesisClientFactory = _ => _kinesisClientMock.Object;
    }

    [TearDown]
    public void TearDown()
    {
        AmazonKinesis.KinesisClientFactory = AmazonKinesis.DefaultKinesisClientFactory;
    }

    [Test]
    public async Task PutRecords_PartialFailure_ThrowErrorOnFailure_False_ReturnsFailedResultWithEntries()
    {
        _kinesisClientMock
            .Setup(c => c.PutRecordsAsync(It.IsAny<PutRecordsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildResponse(
                (true, null, null),
                (false, "ProvisionedThroughputExceededException", "Rate exceeded")));

        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;

        var result = await AmazonKinesis.PutRecords(DefaultInput(), new Connection(), options, default);

        Assert.That(result.Success, Is.False);
        Assert.That(result.FailedRecordCount, Is.EqualTo(1));
        Assert.That(result.Entries, Has.Count.EqualTo(2));
        Assert.That(result.Entries[0].IsSuccessful, Is.True);
        Assert.That(result.Entries[1].IsSuccessful, Is.False);
        Assert.That(result.Entries[1].ErrorCode, Is.EqualTo("ProvisionedThroughputExceededException"));
        Assert.That(result.Error.Message, Contains.Substring("Failed to deliver 1 records out of 2"));
    }

    [Test]
    public void PutRecords_PartialFailure_ThrowErrorOnFailure_True_ThrowsException()
    {
        _kinesisClientMock
            .Setup(c => c.PutRecordsAsync(It.IsAny<PutRecordsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildResponse(
                (true, null, null),
                (false, "ProvisionedThroughputExceededException", "Rate exceeded")));

        var ex = Assert.ThrowsAsync<Exception>(() =>
        AmazonKinesis.PutRecords(DefaultInput(), new Connection(), DefaultOptions(), default));

        Assert.That(ex.Message, Contains.Substring("Failed to deliver 1 records out of 2"));
        Assert.That(ex.Message, Contains.Substring("ProvisionedThroughputExceededException"));
        Assert.That(ex.Message, Contains.Substring("Rate exceeded"));
    }

    private static PutRecordsResponse BuildResponse(params (bool Success, string ErrorCode, string ErrorMessage)[] records)
    {
        var entries = records.Select(r => new PutRecordsResultEntry
        {
            ShardId = r.Success ? "shardId-000000000001" : null,
            SequenceNumber = r.Success ? "49590338271490256608559692540925702759324208523137515522" : null,
            ErrorCode = r.Success ? null : r.ErrorCode,
            ErrorMessage = r.Success ? null : r.ErrorMessage,
        }).ToList();

        return new PutRecordsResponse
        {
            Records = entries,
            FailedRecordCount = records.Count(r => !r.Success),
        };
    }

    private static Input DefaultInput() => new()
    {
        StreamName = "test-stream",
        Records = new List<LogEntry>
    {
        new() { Data = "message 1", PartitionKey = "partition-1" },
        new() { Data = "message 2", PartitionKey = "partition-2" },
    },
    };

    private static Options DefaultOptions() => new()
    {
        ThrowErrorOnFailure = true,
        ErrorMessageOnFailure = string.Empty,
    };
}