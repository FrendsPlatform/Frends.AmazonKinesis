using System.Collections.Generic;

namespace Frends.AmazonKinesis.PutRecords.Definitions;

/// <summary>
/// Result of the task.
/// </summary>
public class Result
{
    public Result(bool success, List<RecordResult> entries, int? failedCount, Error error = null)
    {
        Success = success;
        Entries = entries;
        FailedRecordCount = failedCount;
        Error = error;
    }

    /// <summary>
    /// Indicates if the task completed successfully.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// A list of results for each individual record sent. Use this to inspect which specific records succeeded or failed.
    /// </summary>
    /// <example>[ { "IsSuccessful": true, "ShardId": "shard-001" }, { "IsSuccessful": false, "ErrorCode": "ProvisionedThroughputExceededException" } ]</example>
    public List<RecordResult> Entries { get; }

    /// <summary>
    /// The total number of records that failed to be written to the stream. If this is greater than 0, the overall Success will be false.
    /// </summary>
    /// <example>1</example>
    public int? FailedRecordCount { get; }

    /// <summary>
    /// Error that occurred during task execution.
    /// </summary>
    /// <example>object { string Message, Exception AdditionalInfo }</example>
    public Error Error { get; set; }
}