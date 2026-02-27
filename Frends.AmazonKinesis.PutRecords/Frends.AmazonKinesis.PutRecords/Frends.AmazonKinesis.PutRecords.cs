using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Frends.AmazonKinesis.PutRecords.Definitions;
using Frends.AmazonKinesis.PutRecords.Helpers;

namespace Frends.AmazonKinesis.PutRecords;

/// <summary>
/// Task Class for AmazonKinesis operations.
/// </summary>
public static class AmazonKinesis
{
    /// <summary>
    /// Factory for creating Kinesis clients. Can be overridden in tests to provide a mock client.
    /// Internal visibility allows test projects to access it via InternalsVisibleTo attribute.
    /// </summary>
    internal static Func<Connection, IAmazonKinesis> DefaultKinesisClientFactory { get; set; } =
        connection => new AmazonKinesisClient(
            connection.AwsAccessKeyId,
            connection.AwsSecretAccessKey,
            RegionSelection(connection.Region));

    internal static Func<Connection, IAmazonKinesis> KinesisClientFactory { get; set; } = DefaultKinesisClientFactory;

    /// <summary>
    /// Frends Task for sending data records to an Amazon Kinesis Data Stream
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends-AmazonKinesis-PutRecords)
    /// </summary>
    /// <param name="input">Essential parameters.</param>
    /// <param name="connection">Connection parameters.</param>
    /// <param name="options">Additional parameters.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, List&lt;RecordResult&gt; Entries, int FailedRecordCount, object Error { string Message, Exception AdditionalInfo } }</returns>
    public static async Task<Result> PutRecords(
    [PropertyTab] Input input,
    [PropertyTab] Connection connection,
    [PropertyTab] Options options,
    CancellationToken cancellationToken)
    {
        var recordResults = new List<RecordResult>();
        var streams = new List<MemoryStream>();
        try
        {
            using var client = KinesisClientFactory(connection);

            var entries = new List<PutRecordsRequestEntry>();
            foreach (var record in input.Records)
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(record.Data), false);
                streams.Add(ms);
                entries.Add(new PutRecordsRequestEntry
                {
                    Data = ms,
                    PartitionKey = record.PartitionKey,
                });
            }

            var request = new PutRecordsRequest
            {
                StreamName = input.StreamName,
                Records = entries,
            };

            var response = await client.PutRecordsAsync(request, cancellationToken);

            foreach (var res in response.Records)
            {
                recordResults.Add(new RecordResult
                {
                    IsSuccessful = string.IsNullOrEmpty(res.ErrorCode),
                    ErrorCode = res.ErrorCode,
                    ErrorMessage = res.ErrorMessage,
                    ShardId = res.ShardId,
                    SequenceNumber = res.SequenceNumber,
                });
            }

            int failedCount = response.FailedRecordCount ?? 0;
            if (failedCount > 0)
            {
                var sb = new StringBuilder();
                sb.Append($"Failed to deliver {failedCount} records out of {input.Records.Count}. ");
                var distinctErrors = recordResults
                    .Where(r => !r.IsSuccessful)
                    .Select(r => $"{r.ErrorCode}: {r.ErrorMessage}")
                    .Distinct();
                sb.Append("Details: " + string.Join(" | ", distinctErrors));
                return ErrorHandler.Handle(new Exception(sb.ToString()), options.ThrowErrorOnFailure, options.ErrorMessageOnFailure, failedCount, recordResults);
            }

            return new Result(true, recordResults, failedCount, null);
        }
        catch (Exception e)
        {
            return ErrorHandler.Handle(e, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
        finally
        {
            foreach (var ms in streams) ms.Dispose();
        }
    }

    [ExcludeFromCodeCoverage(Justification = "can only test eu-central-1")]
    private static RegionEndpoint RegionSelection(Region region)
    {
        return region switch
        {
            Region.AfSouth1 => RegionEndpoint.AFSouth1,
            Region.ApEast1 => RegionEndpoint.APEast1,
            Region.ApNortheast1 => RegionEndpoint.APNortheast1,
            Region.ApNortheast2 => RegionEndpoint.APNortheast2,
            Region.ApNortheast3 => RegionEndpoint.APNortheast3,
            Region.ApSouth1 => RegionEndpoint.APSouth1,
            Region.ApSoutheast1 => RegionEndpoint.APSoutheast1,
            Region.ApSoutheast2 => RegionEndpoint.APSoutheast2,
            Region.CaCentral1 => RegionEndpoint.CACentral1,
            Region.CnNorth1 => RegionEndpoint.CNNorth1,
            Region.CnNorthWest1 => RegionEndpoint.CNNorthWest1,
            Region.EuCentral1 => RegionEndpoint.EUCentral1,
            Region.EuNorth1 => RegionEndpoint.EUNorth1,
            Region.EuSouth1 => RegionEndpoint.EUSouth1,
            Region.EuWest1 => RegionEndpoint.EUWest1,
            Region.EuWest2 => RegionEndpoint.EUWest2,
            Region.EuWest3 => RegionEndpoint.EUWest3,
            Region.MeSouth1 => RegionEndpoint.MESouth1,
            Region.SaEast1 => RegionEndpoint.SAEast1,
            Region.UsEast1 => RegionEndpoint.USEast1,
            Region.UsEast2 => RegionEndpoint.USEast2,
            Region.UsWest1 => RegionEndpoint.USWest1,
            Region.UsWest2 => RegionEndpoint.USWest2,
            _ => RegionEndpoint.EUWest1,
        };
    }
}
