using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.Favorites.Dtos;

namespace KittysolomaMap.Application.Favorites.Commands;

public record CreateFavoriteCommand : CommandBase<FavoriteDto>
{
    public required string Description { get; set; }
    public required Guid UserId { get; set; }
    public required long NodeId { get; set; }
}