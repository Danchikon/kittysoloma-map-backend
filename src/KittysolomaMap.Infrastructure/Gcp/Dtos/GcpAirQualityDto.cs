namespace KittysolomaMap.Infrastructure.Gcp.Dtos;

public record GcpAirQualityDto 
{
    public required  DateTime DateTime { get; init; }
    public required string RegionCode { get; init; }
    public ICollection<GcpAirQualityIndexDto> Indexes { get; init; } = new List<GcpAirQualityIndexDto>();
}