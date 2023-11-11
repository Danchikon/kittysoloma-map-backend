namespace KittysolomaMap.Infrastructure.Aws;

public record AwsCredentialsOptions
{
    public const string Section = "AwsCredentials";
    
    public required string AccessKey { get; init; }
    public required string AccessSecret { get; init; }
    public required string Region { get; init; }
};