namespace KittysolomaMap.Domain.Geo;

public class WayEntity 
{
    public required long Id { get; init; }
    public ICollection<WayTagEntity> Tags { get; set; } = new List<WayTagEntity>();
    
    public ICollection<NodeEntity> Nodes { get; set; } = new List<NodeEntity>();
}