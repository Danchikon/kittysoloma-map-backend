using System.Reflection;
using System.Text.Json.Serialization;
using HotChocolate.Types.Pagination;
using KittysolomaMap.Api.GraphQl;
using KittysolomaMap.Api.GraphQl.Interceptors;
using Microsoft.EntityFrameworkCore;
using KittysolomaMap.Infrastructure.Persistence;

namespace KittysolomaMap.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IHostEnvironment environment
        )
    {
        services.AddRestApi(environment);
        services.AddGraphQlApi<Query<KittysolomaMapDbContext>, KittysolomaMapDbContext>(environment);

        return services;
    }
    
    public static IServiceCollection AddRestApi(this IServiceCollection services, IHostEnvironment environment)
    {
        services
            .AddControllers()
            .AddJsonOptions(jsonOptions => 
            { 
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
            });
        
        if (environment.IsProduction() is false)
        {
            services.AddSwaggerGen();
        }

        return services;
    }
    
    public static IServiceCollection AddGraphQlApi<TQuery, TDbContext>(this IServiceCollection services, IHostEnvironment environment) 
        where TDbContext : DbContext
        where TQuery : class
    {
        services
            .AddGraphQLServer()
            .AddHttpRequestInterceptor<ExceptionalHttpRequestInterceptor>()
            .RegisterDbContext<TDbContext>(DbContextKind.Pooled)
            .SetPagingOptions(new PagingOptions
            {
                DefaultPageSize = 10,
                IncludeTotalCount = true,
                MaxPageSize = 100
            })
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .AddSpatialTypes()
            .AddSpatialFiltering()
            .AddSpatialProjections()
            .AllowIntrospection(environment.IsProduction() is false)
            .AddQueryType<TQuery>();

        return services;
    }
}