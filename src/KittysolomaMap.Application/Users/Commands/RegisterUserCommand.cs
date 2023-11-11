using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.Users.Dto;

namespace KittysolomaMap.Application.Users.Commands;

public record RegisterUserCommand : CommandBase<LoginUserResponseDto>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required decimal? Latitude { get; init; }
    public required decimal? Longitude { get; init; }
}