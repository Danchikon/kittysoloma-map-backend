namespace KittysolomaMap.Infrastructure.Gcp.Dtos;

public record GetGcpAirQualityRequestDto
{
    public required GcpAirQualityLocationDto Location { get; init; }
};