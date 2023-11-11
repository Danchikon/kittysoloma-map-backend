using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.FileStorage;

namespace KittysolomaMap.Application.Avatars.Queries;

public record GetUserAvatarQuery : QueryBase<FileDto>
{
    public required string Id { get; init; }
};