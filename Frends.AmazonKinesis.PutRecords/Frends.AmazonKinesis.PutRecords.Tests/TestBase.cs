using System;
using dotenv.net;
using Frends.AmazonKinesis.PutRecords.Definitions;

namespace Frends.AmazonKinesis.PutRecords.Tests;

public abstract class TestBase
{
    protected TestBase()
    {
        DotEnv.Load();
        AccessKey = Environment.GetEnvironmentVariable("AMAZON_KINESIS_ACCESS_KEY");
        SecretAccessKey = Environment.GetEnvironmentVariable("AMAZON_KINESIS_SECRET_ACCESS_KEY");
    }

    private string AccessKey { get; set; }

    private string SecretAccessKey { get; set; }

    protected static Options DefaultOptions() => new()
    {
        ThrowErrorOnFailure = true,
    };

    protected static Input DefaultInput() => new()
    {
        StreamName = "fsp",
    };

    protected Connection DefaultConnection() => new()
    {
        AwsAccessKeyId = AccessKey,
        AwsSecretAccessKey = SecretAccessKey,
        Region = Region.EuCentral1,
    };
}
