using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.Geo;

namespace KittysolomaMap.Infrastructure.Persistence.EntityConfigurations;

public class WayEntityConfiguration : IEntityTypeConfiguration<WayEntity>
{
    public void Configure(EntityTypeBuilder<WayEntity> builder)
    {
        builder.HasMany(way => way.Tags);
        
        builder
            .HasMany(way => way.Nodes)
            .WithMany(node => node.Ways)
            .UsingEntity<NodeWayEntity>();
    }
}