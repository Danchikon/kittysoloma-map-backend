using KittysolomaMap.Application.Dtos;

namespace KittysolomaMap.Application.Users.Dto;

public record LoginUserResponseDto
{
    public required string AccessToken { get; init; }
    public required UserDto User { get; init; }
}