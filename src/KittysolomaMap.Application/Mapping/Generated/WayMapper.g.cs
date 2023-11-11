using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.Mapping.Mappers;
using KittysolomaMap.Domain.Geo;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Mapping.Mappers
{
    public partial class WayMapper : IWayMapper
    {
        public Expression<Func<WayEntity, WayDto>> ProjectToDto => p1 => new WayDto()
        {
            Id = p1.Id,
            Nodes = p1.Nodes.Select<NodeEntity, NodeDto>(p2 => new NodeDto()
            {
                Id = p2.Id,
                Location = p2.Location,
                Tags = p2.Tags.Select<NodeTagEntity, NodeTagDto>(p3 => new NodeTagDto()
                {
                    NodeId = p3.NodeId,
                    Name = p3.Name,
                    Value = p3.Value,
                    Id = p3.Id,
                    CreatedAt = p3.CreatedAt,
                    UpdatedAt = p3.UpdatedAt
                }).ToList<NodeTagDto>()
            }).ToList<NodeDto>(),
            Tags = p1.Tags.Select<WayTagEntity, WayTagDto>(p4 => new WayTagDto()
            {
                WayId = p4.WayId,
                Name = p4.Name,
                Value = p4.Value,
                Id = p4.Id,
                CreatedAt = p4.CreatedAt,
                UpdatedAt = p4.UpdatedAt
            }).ToList<WayTagDto>()
        };
        public WayDto MapToDto(WayEntity p5)
        {
            return p5 == null ? null : new WayDto()
            {
                Id = p5.Id,
                Nodes = funcMain1(p5.Nodes),
                Tags = funcMain4(p5.Tags)
            };
        }
        
        private ICollection<NodeDto> funcMain1(ICollection<NodeEntity> p6)
        {
            if (p6 == null)
            {
                return null;
            }
            ICollection<NodeDto> result = new List<NodeDto>(p6.Count);
            
            IEnumerator<NodeEntity> enumerator = p6.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                NodeEntity item = enumerator.Current;
                result.Add(funcMain2(item));
            }
            return result;
            
        }
        
        private ICollection<WayTagDto> funcMain4(ICollection<WayTagEntity> p9)
        {
            if (p9 == null)
            {
                return null;
            }
            ICollection<WayTagDto> result = new List<WayTagDto>(p9.Count);
            
            IEnumerator<WayTagEntity> enumerator = p9.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                WayTagEntity item = enumerator.Current;
                result.Add(item == null ? null : new WayTagDto()
                {
                    WayId = item.WayId,
                    Name = item.Name,
                    Value = item.Value,
                    Id = item.Id,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            return result;
            
        }
        
        private NodeDto funcMain2(NodeEntity p7)
        {
            return p7 == null ? null : new NodeDto()
            {
                Id = p7.Id,
                Location = p7.Location == null ? null : new Point(p7.Location.Coordinate)
                {
                    X = p7.Location.X,
                    Y = p7.Location.Y,
                    Z = p7.Location.Z,
                    M = p7.Location.M,
                    UserData = p7.Location.UserData,
                    SRID = p7.Location.SRID
                },
                Tags = funcMain3(p7.Tags)
            };
        }
        
        private ICollection<NodeTagDto> funcMain3(ICollection<NodeTagEntity> p8)
        {
            if (p8 == null)
            {
                return null;
            }
            ICollection<NodeTagDto> result = new List<NodeTagDto>(p8.Count);
            
            IEnumerator<NodeTagEntity> enumerator = p8.GetEnumerator();
            
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