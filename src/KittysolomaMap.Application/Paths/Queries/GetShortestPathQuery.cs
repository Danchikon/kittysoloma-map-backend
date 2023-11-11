using KittysolomaMap.Application.Common.Mediator;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Paths.Queries;

public record GetShortestPathQuery : QueryBase<ICollection<Point>>
{
    public required Point StartLocation { get; init; }
    public required Point EndLocation { get; init; }
}