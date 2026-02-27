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
        SecretAccessKey = Environment.GetEnvironmentVariable("AMAZON_KINESIS_SECRET_ACCES_KEY");
    }

    protected string AccessKey { get; private set; }

    protected string SecretAccessKey { get; private set; }

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