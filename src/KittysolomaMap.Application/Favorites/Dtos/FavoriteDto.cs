using KittysolomaMap.Application.Common.Dtos;
using KittysolomaMap.Application.Dtos;

namespace KittysolomaMap.Application.Favorites.Dtos;

public record FavoriteDto : EntityDtoBase
{
    public required string Description { get; init; }
    public required Guid UserId { get; init; }
    public UserDto? User { get; init; }
    public required long NodeId { get; init; }
    public NodeDto? Node { get; init; }
}