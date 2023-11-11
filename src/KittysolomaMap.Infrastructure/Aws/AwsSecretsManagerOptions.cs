namespace KittysolomaMap.Infrastructure.Aws;

public record AwsSecretsManagerOptions
{
    public const string Section = "AwsSecretsManager";
    
    public required string SecretId { get; init; }
    public required string Region { get; init; }
}