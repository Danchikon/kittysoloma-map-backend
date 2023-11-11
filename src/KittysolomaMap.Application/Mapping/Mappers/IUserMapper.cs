using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using Mapster;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Application.Mapping.Mappers;

[Mapper]
public interface IUserMapper
{
    Expression<Func<UserEntity, UserDto>> ProjectToDto { get; }
    
    UserDto MapToDto(UserEntity customer);
}