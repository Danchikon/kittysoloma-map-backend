namespace KittysolomaMap.Api.Dtos.Avatars;

public record UploadAvatarRequestDto
{
    public required IFormFile File { get; init; }
    public required Guid UserId { get; init; }
}