using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Domain.Geo;
using Mapster;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Application.Mapping.Mappers;

[Mapper]
public interface INodeMapper
{
    Expression<Func<NodeEntity, NodeDto>> ProjectToDto { get; }
    
    NodeDto MapToDto(NodeEntity customer);
}