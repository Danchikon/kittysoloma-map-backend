using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Paths;

public interface IShortestPathService
{
    Task<ICollection<Point>> GetShortestPathAsync(
        Point startLocation,
        Point endLocation,
        CancellationToken cancellationToken
        );
}