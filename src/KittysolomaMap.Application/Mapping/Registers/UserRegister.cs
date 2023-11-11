using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Domain.User;
using Mapster;

namespace KittysolomaMap.Application.Mapping.Registers;

public class UserRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserEntity, UserDto>()
            .Map(
                dto => dto.LastLoginLatitude, 
                entity => entity.LastLoginLocation == null ? (decimal?)null : Convert.ToDecimal(entity.LastLoginLocation!.Y)
                )
            .Map(
                dto => dto.LastLoginLongitude, 
                entity => entity.LastLoginLocation == null ? (decimal?)null : Convert.ToDecimal(entity.LastLoginLocation!.X)
                );
    }
}