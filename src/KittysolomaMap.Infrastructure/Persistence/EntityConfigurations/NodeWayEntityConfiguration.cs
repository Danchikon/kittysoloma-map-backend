using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.Geo;

namespace KittysolomaMap.Infrastructure.Persistence.EntityConfigurations;

public class NodeWayEntityConfiguration : IEntityTypeConfiguration<NodeWayEntity>
{
    public void Configure(EntityTypeBuilder<NodeWayEntity> builder)
    {
        builder.HasKey(nodeWay => new { nodeWay.WayId, nodeWay.NodeId });
    }
}