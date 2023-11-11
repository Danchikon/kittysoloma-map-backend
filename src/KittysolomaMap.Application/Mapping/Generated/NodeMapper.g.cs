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
    public partial class NodeMapper : INodeMapper
    {
        public Expression<Func<NodeEntity, NodeDto>> ProjectToDto => p1 => new NodeDto()
        {
            Id = p1.Id,
            Location = p1.Location,
            Tags = p1.Tags.Select<NodeTagEntity, NodeTagDto>(p2 => new NodeTagDto()
            {
                NodeId = p2.NodeId,
                Name = p2.Name,
                Value = p2.Value,
                Id = p2.Id,
                CreatedAt = p2.CreatedAt,
                UpdatedAt = p2.UpdatedAt
            }).ToList<NodeTagDto>()
        };
        public NodeDto MapToDto(NodeEntity p3)
        {
            return p3 == null ? null : new NodeDto()
            {
                Id = p3.Id,
                Location = p3.Location == null ? null : new Point(p3.Location.Coordinate)
                {
                    X = p3.Location.X,
                    Y = p3.Location.Y,
                    Z = p3.Location.Z,
                    M = p3.Location.M,
                    UserData = p3.Location.UserData,
                    SRID = p3.Location.SRID
                },
                Tags = funcMain1(p3.Tags)
            };
        }
        
        private ICollection<NodeTagDto> funcMain1(ICollection<NodeTagEntity> p4)
        {
            if (p4 == null)
            {
                return null;
            }
            ICollection<NodeTagDto> result = new List<NodeTagDto>(p4.Count);
            
            IEnumerator<NodeTagEntity> enumerator = p4.GetEnumerator();
            
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