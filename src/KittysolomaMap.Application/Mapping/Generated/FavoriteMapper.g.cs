using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.Favorites.Dtos;
using KittysolomaMap.Application.Mapping.Mappers;
using KittysolomaMap.Domain.Favorite;
using KittysolomaMap.Domain.Geo;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Mapping.Mappers
{
    public partial class FavoriteMapper : IFavoriteMapper
    {
        public Expression<Func<FavoriteEntity, FavoriteDto>> ProjectToDto => p1 => new FavoriteDto()
        {
            Description = p1.Description,
            UserId = p1.UserId,
            User = p1.User == null ? null : new UserDto()
            {
                FirstName = p1.User.FirstName,
                LastName = p1.User.LastName,
                Email = p1.User.Email,
                AvatarUrl = p1.User.AvatarUrl,
                Role = p1.User.Role,
                LastLoginLongitude = p1.User.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p1.User.LastLoginLocation.X),
                LastLoginLatitude = p1.User.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p1.User.LastLoginLocation.Y),
                Id = p1.User.Id,
                CreatedAt = p1.User.CreatedAt,
                UpdatedAt = p1.User.UpdatedAt
            },
            NodeId = p1.NodeId,
            Node = p1.Node == null ? null : new NodeDto()
            {
                Id = p1.Node.Id,
                Location = p1.Node.Location,
                Tags = p1.Node.Tags.Select<NodeTagEntity, NodeTagDto>(p2 => new NodeTagDto()
                {
                    NodeId = p2.NodeId,
                    Name = p2.Name,
                    Value = p2.Value,
                    Id = p2.Id,
                    CreatedAt = p2.CreatedAt,
                    UpdatedAt = p2.UpdatedAt
                }).ToList<NodeTagDto>()
            },
            Id = p1.Id,
            CreatedAt = p1.CreatedAt,
            UpdatedAt = p1.UpdatedAt
        };
        public FavoriteDto MapToDto(FavoriteEntity p3)
        {
            return p3 == null ? null : new FavoriteDto()
            {
                Description = p3.Description,
                UserId = p3.UserId,
                User = p3.User == null ? null : new UserDto()
                {
                    FirstName = p3.User.FirstName,
                    LastName = p3.User.LastName,
                    Email = p3.User.Email,
                    AvatarUrl = p3.User.AvatarUrl,
                    Role = p3.User.Role,
                    LastLoginLongitude = p3.User.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p3.User.LastLoginLocation.X),
                    LastLoginLatitude = p3.User.LastLoginLocation == null ? (decimal?)null : (decimal?)Convert.ToDecimal(p3.User.LastLoginLocation.Y),
                    Id = p3.User.Id,
                    CreatedAt = p3.User.CreatedAt,
                    UpdatedAt = p3.User.UpdatedAt
                },
                NodeId = p3.NodeId,
                Node = funcMain1(p3.Node),
                Id = p3.Id,
                CreatedAt = p3.CreatedAt,
                UpdatedAt = p3.UpdatedAt
            };
        }
        
        private NodeDto funcMain1(NodeEntity p4)
        {
            return p4 == null ? null : new NodeDto()
            {
                Id = p4.Id,
                Location = p4.Location == null ? null : new Point(p4.Location.Coordinate)
                {
                    X = p4.Location.X,
                    Y = p4.Location.Y,
                    Z = p4.Location.Z,
                    M = p4.Location.M,
                    UserData = p4.Location.UserData,
                    SRID = p4.Location.SRID
                },
                Tags = funcMain2(p4.Tags)
            };
        }
        
        private ICollection<NodeTagDto> funcMain2(ICollection<NodeTagEntity> p5)
        {
            if (p5 == null)
            {
                return null;
            }
            ICollection<NodeTagDto> result = new List<NodeTagDto>(p5.Count);
            
            IEnumerator<NodeTagEntity> enumerator = p5.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                NodeTagEntity item = enumerator.Current;
                result.Add(item == null ? null : new NodeTagDto()
                {
                    NodeId = item.NodeId,
                    Name = item.Name,
                    Value = item.Value,
                    Id = item.Id,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            return result;
            
        }
    }
}