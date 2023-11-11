using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.Favorite;
using KittysolomaMap.Infrastructure.Common.Persistence.EntityConfigurations;

namespace KittysolomaMap.Infrastructure.Persistence.EntityConfigurations;

public class FavoriteEntityConfiguration : EntityConfigurationBase<FavoriteEntity>
{
    public override void Configure(EntityTypeBuilder<FavoriteEntity> builder)
    {
        base.Configure(builder);
    }
}