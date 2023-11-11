using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.Paths.Queries;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Paths.QueryHandlers;

public class GetShortestPathQueryHandler : QueryHandlerBase<GetShortestPathQuery, ICollection<Point>>
{
    private readonly IShortestPathService _shortestPathService;

    public GetShortestPathQueryHandler(IShortestPathService shortestPathService)
    {
        _shortestPathService = shortestPathService;
    }

    public override async Task<ICollection<Point>> Handle(GetShortestPathQuery query, CancellationToken cancellationToken)
    {
        return await _shortestPathService.GetShortestPathAsync(
            query.StartLocation, 
            query.EndLocation,
            cancellationToken
            );
    }
}