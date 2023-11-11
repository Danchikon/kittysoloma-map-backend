using KittysolomaMap.Domain.Common;
using KittysolomaMap.Domain.Common.Entities;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Domain.User;

public class UserEntity : EntityBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public required string PasswordHash { get; set; }
    public required UserRole Role { get; set; }
    public Point? LastLoginLocation { get; set; }
}