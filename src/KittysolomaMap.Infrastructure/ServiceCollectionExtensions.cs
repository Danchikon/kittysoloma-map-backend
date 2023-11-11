using System.Reflection;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Refit;
using KittysolomaMap.Application.FileStorage;
using KittysolomaMap.Application.Paths;
using KittysolomaMap.Application.Security;
using KittysolomaMap.Application.Users;
using KittysolomaMap.Domain.Common.Repositories;
using KittysolomaMap.Domain.Common.UnitOfWork;
using KittysolomaMap.Domain.Favorite;
using KittysolomaMap.Domain.User;
using KittysolomaMap.Infrastructure.Aws;
using KittysolomaMap.Infrastructure.Common.Persistence.Interceptors;
using KittysolomaMap.Infrastructure.Common.Repositories;
using KittysolomaMap.Infrastructure.Common.UnitOfWork;
using KittysolomaMap.Infrastructure.Gcp;
using KittysolomaMap.Infrastructure.Gcp.Refit;
using KittysolomaMap.Infrastructure.Overpass;
using KittysolomaMap.Infrastructure.Overpass.Refit;
using KittysolomaMap.Infrastructure.Paths;
using KittysolomaMap.Infrastructure.Persistence;
using KittysolomaMap.Infrastructure.Security;
using KittysolomaMap.Infrastructure.Users;

namespace KittysolomaMap.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
        )
    {
        services
            .AddOptions<PasswordHasherOptions>()
            .BindConfiguration(PasswordHasherOptions.Section);
        
        services.TryAddScoped<IShortestPathService, ShortestPathService>();
        services.TryAddSingleton<IPasswordHasher, PasswordHasher>();
        services.TryAddSingleton<IAvatarGenerator, AvatarGenerator>();
        
        services.AddOverpassApi(configuration);
        services.AddGcpAirQualityApi(configuration);

        if (environment.IsDevelopment())
        {
            services.AddMinioFileStorage();
        }
        else
        {
            services.AddAws(configuration);
            services.AddAwsFileStorage();
        }

        services.TryAddScoped<IUnitOfWork, EfUnitOfWork<KittysolomaMapDbContext>>();
        services.AddRepositories();

        services.AddNpgsqlDataSourceBuilder(configuration, environment);
        services.AddNpgsqlConnection();
        services.AddPooledEfPostgres<KittysolomaMapDbContext>(environment);

        return services;
    }
    
    public static IServiceCollection AddGcpAirQualityApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<GcpOptions>()
            .BindConfiguration(GcpOptions.Section);
        
        var gcpAirQualityOptions = configuration
            .GetRequiredSection(GcpAirQualityOptions.Section)
            .Get<GcpAirQualityOptions>();
        
        if (gcpAirQualityOptions is null)
        {
            throw new InvalidOperationException($"Section {GcpAirQualityOptions.Section} is null");
        }
        
        services
            .AddRefitClient<IGcpAirQualityRefitApi>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(3))
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(gcpAirQualityOptions.Url);
            });

        return services;
    }
    
    public static IServiceCollection AddOverpassApi(this IServiceCollection services, IConfiguration configuration)
    {
        var overpassOptions = configuration
            .GetRequiredSection(OverpassOptions.Section)
            .Get<OverpassOptions>();
        
        if (overpassOptions is null)
        {
            throw new InvalidOperationException($"Section {OverpassOptions.Section} is null");
        }
        
        services
            .AddRefitClient<IOverpassRefitApi>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(3))
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(overpassOptions.Url);
            });

        return services;
    }
    
    public static IServiceCollection AddAws(this IServiceCollection services, IConfiguration configuration)
    {
        var awsCredentialsOptions = configuration
            .GetRequiredSection(AwsCredentialsOptions.Section)
            .Get<AwsCredentialsOptions>();
        
        if (awsCredentialsOptions is null)
        {
            throw new InvalidOperationException($"Section {AwsCredentialsOptions.Section} is null");
        }
        
        services.AddDefaultAWSOptions(new AWSOptions
        {
            Credentials = new BasicAWSCredentials(awsCredentialsOptions.AccessKey, awsCredentialsOptions.AccessSecret),
            Region = RegionEndpoint.GetBySystemName(awsCredentialsOptions.Region),
        });

        return services;
    }
    
    public static IServiceCollection AddAwsFileStorage(this IServiceCollection services)
    {
        services.AddAWSService<IAmazonS3>();
        services.AddSingleton<IFileStorage, AwsFileStorage>();

        return services;
    }
    
    public static IServiceCollection AddMinioFileStorage(this IServiceCollection services)
    {
        services.TryAddSingleton<IAmazonS3>(provider =>
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.EUCentral1, 
                ServiceURL = "http://localhost:9000",
                ForcePathStyle = true 
            };
            
            return new AmazonS3Client("minioadmin", "minioadmin", config);
        });
        services.TryAddSingleton<IFileStorage, AwsFileStorage>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services) 
    {
        services.TryAddScoped<IRepository<FavoriteEntity>, EfRepository<FavoriteEntity, KittysolomaMapDbContext>>();
        services.TryAddScoped<IRepository<UserEntity>, EfRepository<UserEntity, KittysolomaMapDbContext>>();

        return services;
    }
    
    public static IServiceCollection AddNpgsqlDataSourceBuilder(
        this IServiceCollection services, 
        IConfiguration configuration,
        IHostEnvironment environment
    ) 
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        services.TryAddSingleton(_ =>
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            
            dataSourceBuilder.UseNetTopologySuite();
            
            if (environment.IsDevelopment())
            {
                dataSourceBuilder.EnableParameterLogging();
            }
            
            return dataSourceBuilder;
        });

        return services;
    }
    
    public static IServiceCollection AddNpgsqlConnection(this IServiceCollection services) 
    {
        services.TryAddSingleton(provider =>
        {
            var dataSourceBuilder = provider.GetRequiredService<NpgsqlDataSourceBuilder>();

            return new NpgsqlConnection(dataSourceBuilder.Build().ConnectionString);
        });

        return services;
    }
    
    public static IServiceCollection AddPooledEfPostgres<TDbContext>(
        this IServiceCollection services, 
        IHostEnvironment environment
        ) where TDbContext: DbContext
    {
        services.AddPooledDbContextFactory<TDbContext>(dbContextOptionsBuilder =>
        {
            using var provider = services.BuildServiceProvider();
            var dataSourceBuilder = provider.GetRequiredService<NpgsqlDataSourceBuilder>();
            
            if (environment.IsProduction() is false)
            {
                dbContextOptionsBuilder.EnableDetailedErrors();
                dbContextOptionsBuilder.EnableSensitiveDataLogging();
            }
            
            dbContextOptionsBuilder.UseSnakeCaseNamingConvention();
            dbContextOptionsBuilder.AddInterceptors(new ChangesTrackingInterceptor());
            dbContextOptionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            
            dbContextOptionsBuilder.UseNpgsql(dataSourceBuilder.Build(), npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite();
                npgsqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                npgsqlOptions.MigrationsHistoryTable("migrations");
            });
        });

        services.AddScoped<TDbContext>(provider =>
        {
            var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TDbContext>>();
           
            return dbContextFactory.CreateDbContext();
        });

        return services;
    }
}