using Mapster;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Mapping.Registers;

public class PointRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Point, Point>()
            .ConstructUsing(point => new Point(point.Coordinate));
    }
}