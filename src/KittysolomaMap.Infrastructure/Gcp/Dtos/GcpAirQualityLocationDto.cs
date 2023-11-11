namespace KittysolomaMap.Infrastructure.Gcp.Dtos;

public record GcpAirQualityLocationDto
{
    public required decimal Latitude { get; init; }
    public required decimal Longitude { get; init; }
}