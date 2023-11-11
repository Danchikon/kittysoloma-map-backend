using System.Linq.Expressions;
using KittysolomaMap.Application.Common.Mediator;
using KittysolomaMap.Application.Favorites.Commands;
using KittysolomaMap.Application.Favorites.Dtos;
using KittysolomaMap.Application.Mapping.Mappers;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using KittysolomaMap.Domain.Common.Repositories;
using KittysolomaMap.Domain.Common.UnitOfWork;
using KittysolomaMap.Domain.Favorite;

namespace KittysolomaMap.Application.Favorites.CommandHandler;

public class CreateFavoriteCommandHandler : CommandHandlerBase<CreateFavoriteCommand, FavoriteDto>
{
    private readonly IRepository<FavoriteEntity> _favoritesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFavoriteMapper _favoriteMapper;

    public CreateFavoriteCommandHandler(
        IFavoriteMapper favoriteMapper, 
        IRepository<FavoriteEntity> favoritesRepository, 
        IUnitOfWork unitOfWork
        )
    {
        _favoriteMapper = favoriteMapper;
        _favoritesRepository = favoritesRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<FavoriteDto> Handle(CreateFavoriteCommand command, CancellationToken cancellationToken)
    {
        var anyFavorite = await _favoritesRepository.AnyAsync(
            new Expression<Func<FavoriteEntity, bool>>[]
            {
                favorite => favorite.UserId == command.UserId,
                favorite => favorite.NodeId == command.NodeId,
            },
            cancellationToken: cancellationToken
        );

        if (anyFavorite)
        {
            throw new BusinessException
            {
                ErrorCode = ErrorCode.FavoriteAlreadyExist,
                ErrorKind = ErrorKind.InvalidOperation
            };
        }
        
        var favoriteEntity = new FavoriteEntity
        {
            Description = command.Description,
            UserId = command.UserId,
            NodeId = command.NodeId
        };
        
        var updatedFavorite = await _favoritesRepository.AddAsync(favoriteEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _favoriteMapper.MapToDto(updatedFavorite);
    }
}