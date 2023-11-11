using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.FileStorage;

namespace KittysolomaMap.Application.Avatars.Commands;

public record UploadUserAvatarCommand : CommandBase<string>
{
    public required FileDto File { get; init; }
    public required string FileUrlBase { get; init; }
    public required Guid UserId { get; init; }
}