using System.Linq.Expressions;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.Favorites.Dtos;
using KittysolomaMap.Domain.Favorite;
using Mapster;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Application.Mapping.Mappers;

[Mapper]
public interface IFavoriteMapper
{
    Expression<Func<FavoriteEntity, FavoriteDto>> ProjectToDto { get; }
    
    FavoriteDto MapToDto(FavoriteEntity customer);
}