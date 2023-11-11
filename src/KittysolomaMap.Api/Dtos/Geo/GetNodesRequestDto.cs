using KittysolomaMap.Domain.Filtering.Nodes;

namespace KittysolomaMap.Api.Dtos.Geo;

public record GetNodesRequestDto
{
    public bool IncludeAirQuality { get; init; }
    public NodesFilterOptions? Filter { get; init; }
}