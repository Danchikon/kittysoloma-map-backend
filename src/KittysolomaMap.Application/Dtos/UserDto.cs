using System.Collections.Immutable;
using KittysolomaMap.Application.Common.Dtos;
using KittysolomaMap.Domain.User;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Dtos;

public record UserDto : EntityDtoBase
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? AvatarUrl { get; set; }
    public required UserRole Role { get; init; }
    public decimal? LastLoginLongitude { get; init; }
    public decimal? LastLoginLatitude { get; init; }
}