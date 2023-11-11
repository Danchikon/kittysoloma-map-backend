namespace KittysolomaMap.Api.Dtos.Favorite;

public record CreateFavoriteDto 
{
    public required string Description { get; set; }
    public required Guid UserId { get; set; }
    public required long NodeId { get; set; }
}