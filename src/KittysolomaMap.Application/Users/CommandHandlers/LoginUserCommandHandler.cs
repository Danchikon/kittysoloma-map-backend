using System.Linq.Expressions;
using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.Mapping.Mappers;
using KittysolomaMap.Application.Security;
using KittysolomaMap.Application.Users.Commands;
using KittysolomaMap.Application.Users.Dto;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using KittysolomaMap.Domain.Common.Repositories;
using KittysolomaMap.Domain.Common.UnitOfWork;
using KittysolomaMap.Domain.User;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Application.Users.CommandHandlers;

public class LoginUserCommandHandler : CommandHandlerBase<LoginUserCommand, LoginUserResponseDto>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserMapper _userMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<UserEntity> _usersRepository;

    public LoginUserCommandHandler(
        IPasswordHasher passwordHasher, 
        IUnitOfWork unitOfWork, 
        IRepository<UserEntity> usersRepository, 
        IUserMapper userMapper
        )
    {
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _usersRepository = usersRepository;
        _userMapper = userMapper;
    }

    public override async Task<LoginUserResponseDto> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var userEntity = await _usersRepository.FirstAsync(
            new Expression<Func<UserEntity, bool>>[] 
            {
                user => user.Email ==command.Email
            }, cancellationToken);
        

        var hashed = _passwordHasher.Hash(command.Password);

        if (userEntity.PasswordHash != hashed)
        {
            throw new BusinessException
            {
                ErrorCode = ErrorCode.InvalidPassword,
                ErrorKind = ErrorKind.PermissionDenied
            };
        }

        userEntity.LastLoginLocation = new Point(Convert.ToDouble(command.Longitude), Convert.ToDouble(command.Longitude));

        await _usersRepository.UpdateAsync(userEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = _userMapper.MapToDto(userEntity);

        return new LoginUserResponseDto
        {
            AccessToken = "raw-token",
            User = userDto
        };
    }
}