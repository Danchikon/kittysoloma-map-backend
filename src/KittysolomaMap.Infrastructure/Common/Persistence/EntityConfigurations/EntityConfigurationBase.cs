using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.Common;
using KittysolomaMap.Domain.Common.Entities;

namespace KittysolomaMap.Infrastructure.Common.Persistence.EntityConfigurations;

public abstract class EntityConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);
        builder.HasIndex(entity => entity.IsActive);
        builder.HasQueryFilter(entity => entity.IsActive);
    }
}