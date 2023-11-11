namespace KittysolomaMap.Infrastructure.Gcp;

public class GcpAirQualityOptions
{
    public const string Section = "GcpAirQuality";
    
    public required string Url { get; init; }
}