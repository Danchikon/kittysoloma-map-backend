using Amazon.Runtime.Internal.Transform;
using KittysolomaMap.Application.Paths;
using KittysolomaMap.Domain.Geo;
using KittysolomaMap.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using QuickGraph;
using QuickGraph.Algorithms;

namespace KittysolomaMap.Infrastructure.Paths;

public class ShortestPathService : IShortestPathService
{
    private readonly KittysolomaMapDbContext _dbContext;

    public ShortestPathService(KittysolomaMapDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<Point>> GetShortestPathAsync(
        Point startLocation, 
        Point endLocation,
        CancellationToken cancellationToken)
    {
        var wayEntities = await _dbContext.Ways
            .Include(way => way.Nodes)
            .Where(way => way.Tags.Any(tag => tag.Name == "highway" && tag.Value == "footway"))
            .ToListAsync(cancellationToken);

        var closestNodeToStart = wayEntities
            .SelectMany(way => way.Nodes)
            .OrderBy(node => node.Location.Distance(startLocation))
            .First();

        var closestNodeToEnd = wayEntities
            .SelectMany(way => way.Nodes)
            .OrderBy(node => node.Location.Distance(endLocation))
            .First();
        
        var dijkstraAlgorithm = new DijkstraAlgorithm();

        foreach (var wayEntity in wayEntities)
        {
            var nodes = wayEntity.Nodes.ToArray();

            for (var i = 1; i < nodes.Length; i++)
            {
                var first = nodes[i - 1];
                var second = nodes[i];
                var distance = first.Location.Distance(second.Location);

                dijkstraAlgorithm.AddVertex(first);
                dijkstraAlgorithm.AddVertex(second);
                dijkstraAlgorithm.AddEdge(first, second, Convert.ToDecimal(distance));
                dijkstraAlgorithm.AddEdge(second, first, Convert.ToDecimal(distance));
            }
        }

        var paths = dijkstraAlgorithm.Dijkstra(closestNodeToStart);

        var path = paths
            .MinBy(path => path
                .Value
                .Item2
                .Last()
                .Location
                .Distance(closestNodeToEnd.Location)
            );

        return path.Value.Item2.Select(node => node.Location).ToList();
    }
}