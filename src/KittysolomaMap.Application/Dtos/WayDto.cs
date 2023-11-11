using NetTopologySuite.Geometries;


namespace KittysolomaMap.Application.Dtos;

public record WayDto
{
    public required long Id { get; init; }
    public ICollection<NodeDto> Nodes { get; init; } = Array.Empty<NodeDto>();
    public ICollection<WayTagDto> Tags { get; init; } = Array.Empty<WayTagDto>();
}