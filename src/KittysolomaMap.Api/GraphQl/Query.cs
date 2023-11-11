using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.Favorites.Dtos;
using KittysolomaMap.Application.Mapping.Mappers;
using KittysolomaMap.Application.Paths.Queries;
using KittysolomaMap.Domain.Favorite;
using KittysolomaMap.Domain.Geo;
using KittysolomaMap.Domain.User;
using KittysolomaMap.Infrastructure.Gcp;
using KittysolomaMap.Infrastructure.Gcp.Dtos;
using KittysolomaMap.Infrastructure.Gcp.Refit;
using MediatR;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Api.GraphQl;

public class Query<TDbContext> where TDbContext : DbContext
{
    public async Task<ICollection<Point>> GetShortestPathAsync(
        GetShortestPathQuery query, 
        [FromServices] IMediator mediator, 
        CancellationToken cancellationToken
        )
    {
        return await mediator.Send(query, cancellationToken);
    }
    
    [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    public IQueryable<FavoriteDto> GetPagedFavorites([FromServices] IFavoriteMapper favoriteMapper, TDbContext dbContext)
    {
        return dbContext.Set<FavoriteEntity>().Select(favoriteMapper.ProjectToDto);
    }
    
    [UsePaging(MaxPageSize = 10000)]
    [UseProjection]
    [UseFiltering]
    public IQueryable<NodeDto> GetPagedNodes([FromServices] INodeMapper nodeMapper, TDbContext dbContext)
    {
        return dbContext.Set<NodeEntity>().Select(nodeMapper.ProjectToDto);
    }
    
    public async Task<NodeDto> GetNodeByIdAsync(
        long id, 
        [FromServices] IGcpAirQualityRefitApi gcpAirQualityRefitApi,
        [FromServices] IOptions<GcpOptions> gcpOptions,
        [FromServices] INodeMapper nodeMapper,
        TDbContext dbContext,
        CancellationToken cancellationToken
    )
    {
        var nodeDto = await dbContext.Set<NodeEntity>().Select(nodeMapper.ProjectToDto).SingleAsync(node => node.Id == id, cancellationToken: cancellationToken);

        var getAirQualityRequest = new GetGcpAirQualityRequestDto
        {
            Location = new GcpAirQualityLocationDto
            {
                Latitude = Convert.ToDecimal(nodeDto.Location.Y),
                Longitude = Convert.ToDecimal(nodeDto.Location.X)
            }
        };

        var getAirQualityResponse = await gcpAirQualityRefitApi.GetAsync(
            gcpOptions.Value.AccessKey,
            getAirQualityRequest,
            cancellationToken
        );
        
        return nodeDto with
        {
            AirQualityCategory = getAirQualityResponse.Content?.Indexes.FirstOrDefault()?.Category
        };
    }
    
    [UsePaging(MaxPageSize = 10000)]
    [UseProjection]
    [UseFiltering]
    public IQueryable<WayDto> GetPagedWays([FromServices] IWayMapper nodeMapper, TDbContext dbContext)
    {
        return dbContext.Set<WayEntity>().Select(nodeMapper.ProjectToDto);
    }
    
    [UseFirstOrDefault]
    [UseProjection]
    [UseFiltering]
    public IQueryable<WayDto> GetFirstOrDefaultWay([FromServices] IWayMapper wayMapper, TDbContext dbContext)
    {
        return dbContext.Set<WayEntity>().Select(wayMapper.ProjectToDto);
    }
    
    [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UserDto> GetPagedUsers([FromServices] IUserMapper userMapper, TDbContext dbContext)
    {
        return dbContext.Set<UserEntity>().Select(userMapper.ProjectToDto);
    }
    
    [UseFirstOrDefault]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UserDto> GetFirstOrDefaultUser([FromServices] IUserMapper userMapper, TDbContext dbContext)
    {
        return dbContext.Set<UserEntity>().Select(userMapper.ProjectToDto);
    }
}