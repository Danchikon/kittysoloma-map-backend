using NetTopologySuite.Geometries;

namespace KittysolomaMap.Domain.Geo;

public class NodeEntity : IComparable<NodeEntity> 
{
    public required long Id { get; init;  }
    public required Point Location { get; set; }
    
    public ICollection<NodeTagEntity> Tags { get; set; } = new List<NodeTagEntity>();
    public ICollection<WayEntity> Ways { get; set; } = new List<WayEntity>();

    public int CompareTo(NodeEntity? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }
        if (ReferenceEquals(null, other))
        {
            return 1;
        }
        
        var idComparison = Id.CompareTo(other.Id);
        
        return idComparison is not 0 ? idComparison : Location.CompareTo(other.Location);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}