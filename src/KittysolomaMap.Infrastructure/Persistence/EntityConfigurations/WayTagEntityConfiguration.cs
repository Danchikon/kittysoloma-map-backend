using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.Geo;
using KittysolomaMap.Infrastructure.Common.Persistence.EntityConfigurations;

namespace KittysolomaMap.Infrastructure.Persistence.EntityConfigurations;

public class WayTagEntityConfiguration : EntityConfigurationBase<WayTagEntity>
{
    public override void Configure(EntityTypeBuilder<WayTagEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(tag => tag.Name);
        builder.HasIndex(tag => tag.Value);
    }
}