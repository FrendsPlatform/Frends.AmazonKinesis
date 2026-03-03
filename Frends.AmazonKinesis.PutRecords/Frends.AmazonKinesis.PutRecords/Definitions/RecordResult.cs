namespace Frends.AmazonKinesis.PutRecords.Definitions
{
    /// <summary>
    /// Detailed result of an individual record delivery attempt.
    /// </summary>
    public class RecordResult
    {
        /// <summary>
        /// Indicates if the record was successfully written to the stream.
        /// </summary>
        /// <example>true</example>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// The identifier of the shard to which the record was assigned. Only populated if successful.
        /// </summary>
        /// <example>shardId-000000000001</example>
        public string ShardId { get; set; }

        /// <summary>
        /// The unique identifier assigned to the record within its shard. Only populated if successful.
        /// </summary>
        /// <example>496272100305415714343135</example>
        public string SequenceNumber { get; set; }

        /// <summary>
        /// The AWS error code if the record failed to be written.
        /// </summary>
        /// <example>ProvisionedThroughputExceededException</example>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Detailed error message explaining why the record failed.
        /// </summary>
        /// <example>Rate exceeded for shard X in stream Y.</example>
        public string ErrorMessage { get; set; }
    }
}
