using System.Reflection;
using KittysolomaMap.Application.Mapping.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KittysolomaMap.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMappers();
        services.AddMediatR(mediatrServiceConfiguration =>
        {
            mediatrServiceConfiguration.Lifetime = ServiceLifetime.Scoped;
            mediatrServiceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        return services;
    }
    
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.TryAddSingleton<IUserMapper, UserMapper>();
        services.TryAddSingleton<IFavoriteMapper, FavoriteMapper>();
        services.TryAddSingleton<INodeMapper, NodeMapper>();
        services.TryAddSingleton<IWayMapper, WayMapper>();
        
        return services;
    }
}