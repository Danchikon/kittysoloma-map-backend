namespace KittysolomaMap.Infrastructure.Gcp.Dtos;

public class GcpAirQualityIndexDto
{
    public required string Code { get; init; }
    public required string DisplayName { get; init; }
    public required int Aqi { get; init; }
    public required string Category { get; init; }
}