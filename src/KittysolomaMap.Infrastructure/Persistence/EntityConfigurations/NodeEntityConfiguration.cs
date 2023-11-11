using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.Geo;

namespace KittysolomaMap.Infrastructure.Persistence.EntityConfigurations;

public class NodeEntityConfiguration : IEntityTypeConfiguration<NodeEntity>
{
    public void Configure(EntityTypeBuilder<NodeEntity> builder)
    {
        builder.HasMany(node => node.Tags);
        
        builder
            .HasMany(node => node.Ways)
            .WithMany(way => way.Nodes)
            .UsingEntity<NodeWayEntity>();
        
        builder
            .Property(node => node.Location)
            .HasColumnType("geometry (point)");
        
        builder.HasIndex(node => node.Location);
    }
}