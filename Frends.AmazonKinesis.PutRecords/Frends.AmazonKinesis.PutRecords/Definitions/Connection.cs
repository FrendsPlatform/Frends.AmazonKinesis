using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.AmazonKinesis.PutRecords.Definitions;

/// <summary>
/// Connection parameters.
/// </summary>
public class Connection
{
    /// <summary>
    /// AWS Access Key ID.
    /// </summary>
    /// <example>AKIAQWERTY...</example>
    [DisplayFormat(DataFormatString = "Text")]
    [PasswordPropertyText]
    public string AwsAccessKeyId { get; set; }

    /// <summary>
    /// AWS Secret Access Key.
    /// </summary>
    /// <example>TVh5hgd3uGY...</example>
    [DisplayFormat(DataFormatString = "Text")]
    [PasswordPropertyText]
    public string AwsSecretAccessKey { get; set; }

    /// <summary>
    /// AWS Kinesis stream region.
    /// </summary>
    /// <example>eu-west-1</example>
    [DefaultValue(Region.EuWest1)]
    public Region Region { get; set; } = Region.EuWest1;
}
