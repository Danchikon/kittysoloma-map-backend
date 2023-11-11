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

public class RegisterUserCommandHandler : CommandHandlerBase<RegisterUserCommand, LoginUserResponseDto>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAvatarGenerator _avatarGenerator;
    private readonly IUserMapper _userMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<UserEntity> _usersRepository;

    public RegisterUserCommandHandler(
        IPasswordHasher passwordHasher, 
        IAvatarGenerator avatarGenerator, 
        IUnitOfWork unitOfWork, 
        IRepository<UserEntity> usersRepository, 
        IUserMapper userMapper
        )
    {
        _passwordHasher = passwordHasher;
        _avatarGenerator = avatarGenerator;
        _unitOfWork = unitOfWork;
        _usersRepository = usersRepository;
        _userMapper = userMapper;
    }

    public override async Task<LoginUserResponseDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var anyUser = await _usersRepository.AnyAsync(
            new Expression<Func<UserEntity, bool>>[]
            {
                user => user.Email == command.Email,
            },
            cancellationToken: cancellationToken
        );

        if (anyUser)
        {
            throw new BusinessException
            {
                ErrorCode = ErrorCode.UserWithSameEmailAlreadyExist,
                ErrorKind = ErrorKind.InvalidOperation
            };
        }
        
        var hashed = _passwordHasher.Hash(command.Password);
        
        var userEntity = new UserEntity
        {
            AvatarUrl = _avatarGenerator.GenerateUrl(),
            LastName = command.LastName,
            FirstName = command.FirstName,
            Email = command.Email,
            PasswordHash = hashed,
            Role = UserRole.Basic,
            LastLoginLocation = new Point(Convert.ToDouble(command.Longitude), Convert.ToDouble(command.Longitude))
        };

        await _usersRepository.AddAsync(userEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = _userMapper.MapToDto(userEntity);

        return new LoginUserResponseDto
        {
            AccessToken = "raw-token",
            User = userDto
        };
    }
}