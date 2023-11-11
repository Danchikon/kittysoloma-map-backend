namespace KittysolomaMap.Infrastructure.Gcp;

public record GcpOptions
{
    public const string Section = "Gcp";
    
    public required string AccessKey { get; init; }
};