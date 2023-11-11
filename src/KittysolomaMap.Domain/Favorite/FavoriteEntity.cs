using KittysolomaMap.Domain.Common.Entities;
using KittysolomaMap.Domain.Geo;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Domain.Favorite;

public class FavoriteEntity : EntityBase
{
    public required string Description { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public required long NodeId { get; set; }
    public NodeEntity? Node { get; set; }
}