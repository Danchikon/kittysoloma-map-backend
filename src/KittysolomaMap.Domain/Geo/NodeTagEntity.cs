using KittysolomaMap.Domain.Common;
using KittysolomaMap.Domain.Common.Entities;

namespace KittysolomaMap.Domain.Geo;

public class NodeTagEntity : EntityBase
{
    public required long NodeId { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
}