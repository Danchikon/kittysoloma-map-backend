using KittysolomaMap.Application.Avatars.Queries;
using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.FileStorage;

namespace KittysolomaMap.Application.Avatars.QueryHandlers;

public class GetUserAvatarQueryHandler : QueryHandlerBase<GetUserAvatarQuery, FileDto>
{
    private readonly IFileStorage _fileStorage;

    public GetUserAvatarQueryHandler(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public override async Task<FileDto> Handle(GetUserAvatarQuery query, CancellationToken cancellationToken)
    {
        var avatar = await _fileStorage.DownloadAsync(FoldersNames.AvatarFolder, query.Id, cancellationToken);

        return avatar;
    }
}