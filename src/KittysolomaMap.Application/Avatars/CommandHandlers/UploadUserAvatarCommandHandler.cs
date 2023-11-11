using System.Linq.Expressions;
using KittysolomaMap.Application.Avatars.Commands;
using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.FileStorage;
using KittysolomaMap.Application.Users;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using KittysolomaMap.Domain.Common.Repositories;
using KittysolomaMap.Domain.Common.UnitOfWork;
using KittysolomaMap.Domain.User;

namespace KittysolomaMap.Application.Avatars.CommandHandlers;

public class UploadUserAvatarCommandHandler : CommandHandlerBase<UploadUserAvatarCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorage _fileStorage;
    private readonly IAvatarGenerator _avatarGenerator;
    private readonly IRepository<UserEntity> _usersRepository;

    public UploadUserAvatarCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<UserEntity> usersRepository,
        IFileStorage fileStorage,
        IAvatarGenerator avatarGenerator
        )
    {
        _unitOfWork = unitOfWork;
        _usersRepository = usersRepository;
        _fileStorage = fileStorage;
        _avatarGenerator = avatarGenerator;
    }

    public override async Task<string> Handle(UploadUserAvatarCommand command, CancellationToken cancellationToken)
    {
        var userEntity = await _usersRepository.FirstOrDefaultAsync(
            new Expression<Func<UserEntity, bool>>[]
            {
                user => user.Id == command.UserId
            }, cancellationToken: cancellationToken);

        if (userEntity is null)
        {
            throw new BusinessException
            {
                ErrorCode = ErrorCode.UserNotFound,
                ErrorKind = ErrorKind.InvalidOperation
            };
        }

        var avatarId = Guid.NewGuid().ToString();

        userEntity.AvatarUrl = $"{command.FileUrlBase}/{avatarId}";
        
        await _usersRepository.UpdateAsync(userEntity, cancellationToken);

        await Task.WhenAll(
            _unitOfWork.SaveChangesAsync(cancellationToken),
            _fileStorage.UploadAsync(FoldersNames.AvatarFolder, avatarId, command.File, cancellationToken)
        );

        return avatarId;
    }
}