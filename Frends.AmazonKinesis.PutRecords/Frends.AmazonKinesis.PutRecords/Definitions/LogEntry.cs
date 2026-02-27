namespace Frends.AmazonKinesis.PutRecords.Definitions
{
    /// <summary>
    /// Represents a single data record to be sent to the Kinesis stream.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// The actual payload of the record. Can be a string, JSON, or any text-based data.
        /// Combined size of Data and PartitionKey must not exceed 10 MB.
        /// </summary>
        /// <example>{ "message": "Transaction completed", "status": 200 }</example>
        public string Data { get; set; }

        /// <summary>
        /// Determines which shard the record is sent to. Records with the same key are guaranteed to be in the same shard and order.
        /// Maximum length is 256 characters.
        /// </summary>
        /// <example>user-12345</example>
        public string PartitionKey { get; set; }
    }
}