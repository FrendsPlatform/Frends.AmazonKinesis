using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Frends.AmazonKinesis.PutRecords.Definitions;

/// <summary>
/// Essential parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// The name of the Kinesis Data Stream to which the records will be sent.
    /// </summary>
    /// <example>MyLogStream</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string StreamName { get; set; }

    /// <summary>
    /// A list of data records to be sent. Each record requires a payload (Data) and a PartitionKey.
    /// Maximum 500 records per request. Total size of all records must not exceed 10 MB.
    /// </summary>
    /// <example>[ { "Data": "Log message", "PartitionKey": "ProcessID_123" } ]</example>
    public List<LogEntry> Records { get; set; }
}