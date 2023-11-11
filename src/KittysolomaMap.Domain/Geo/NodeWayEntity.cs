namespace KittysolomaMap.Domain.Geo;

public class NodeWayEntity
{
    public required long WayId { get; set; }
    public WayEntity? Way { get; set; }
    public required long NodeId { get; set; }
    public NodeEntity? Node { get; set; }
}