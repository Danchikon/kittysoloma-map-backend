using NetTopologySuite.Geometries;


namespace KittysolomaMap.Application.Dtos;

public record NodeDto
{
    public required long Id { get; init; }
    public required Point Location { get; init; }
    public string? AirQualityCategory { get; init; }
    public ICollection<NodeTagDto> Tags { get; init; } = Array.Empty<NodeTagDto>();
}