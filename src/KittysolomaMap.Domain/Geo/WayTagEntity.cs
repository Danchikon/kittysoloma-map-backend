using KittysolomaMap.Domain.Common;
using KittysolomaMap.Domain.Common.Entities;

namespace KittysolomaMap.Domain.Geo;

public class WayTagEntity : EntityBase
{
    public required long WayId { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
}