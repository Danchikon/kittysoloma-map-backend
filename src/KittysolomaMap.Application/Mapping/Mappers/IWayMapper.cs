using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Domain.Geo;
using Mapster;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Application.Mapping.Mappers;

[Mapper]
public interface IWayMapper
{
    Expression<Func<WayEntity, WayDto>> ProjectToDto { get; }
    
    WayDto MapToDto(WayEntity customer);
}