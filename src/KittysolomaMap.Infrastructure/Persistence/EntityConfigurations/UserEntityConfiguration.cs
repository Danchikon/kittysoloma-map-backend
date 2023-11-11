using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KittysolomaMap.Domain.User;
using KittysolomaMap.Infrastructure.Common.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace KittysolomaMap.Infrastructure.Persistence.EntityConfigurations;

public class UserEntityConfiguration : EntityConfigurationBase<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);
        
        builder
            .Property(node => node.LastLoginLocation)
            .HasColumnType("geometry (point)");
        
        builder.HasIndex(entity => entity.Email);
        builder.HasIndex(entity => entity.Role);
    }
}