using System;
using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.Mapping.Mappers;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Application.Mapping.Mappers
{
    public partial class UserMapper : IUserMapper
    {
        public Expression<Func<UserEntity, UserDto>> ProjectToDto => p1 => new UserDto()
        {
            FirstName = p1.FirstName,
            LastName = p1.LastName,
            Email = p1.Email,
            AvatarUrl = p1.AvatarUrl,
            Role = p1.Role,
            LastLoginLongitude = p1.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p1.LastLoginLocation.X),
            LastLoginLatitude = p1.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p1.LastLoginLocation.Y),
            Id = p1.Id,
            CreatedAt = p1.CreatedAt,
            UpdatedAt = p1.UpdatedAt
        };
        public UserDto MapToDto(UserEntity p2)
        {
            return p2 == null ? null : new UserDto()
            {
                FirstName = p2.FirstName,
                LastName = p2.LastName,
                Email = p2.Email,
                AvatarUrl = p2.AvatarUrl,
                Role = p2.Role,
                LastLoginLongitude = p2.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p2.LastLoginLocation.X),
                LastLoginLatitude = p2.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p2.LastLoginLocation.Y),
                Id = p2.Id,
                CreatedAt = p2.CreatedAt,
                UpdatedAt = p2.UpdatedAt
            };
        }
    }
}