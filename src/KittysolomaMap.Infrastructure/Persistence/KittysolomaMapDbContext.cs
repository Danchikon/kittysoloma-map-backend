using System.Reflection;
using Bogus;
using Microsoft.EntityFrameworkCore;
using KittysolomaMap.Domain.Favorite;
using KittysolomaMap.Domain.Geo;
using KittysolomaMap.Domain.User;
using NetTopologySuite.Geometries;

namespace KittysolomaMap.Infrastructure.Persistence;

public class KittysolomaMapDbContext : DbContext
{
    public required DbSet<UserEntity> Users { get; init; }
    public required DbSet<FavoriteEntity> Favorites { get; init; }
    public required DbSet<NodeEntity> Nodes { get; init; }
    public required DbSet<NodeTagEntity> NodeTags { get; init; }
    public required DbSet<WayEntity> Ways { get; init; }
    public required DbSet<WayTagEntity> WayTags { get; init; }
    
    public required DbSet<NodeWayEntity> NodeWays { get; init; }
    
    public KittysolomaMapDbContext(DbContextOptions<KittysolomaMapDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<UserRole>();
        modelBuilder.HasPostgresExtension("postgis");
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        var usersFaker = new Faker<UserEntity>()
            .RuleFor(user => user.Id, Guid.NewGuid)
            .RuleFor(user => user.AvatarUrl, (_, user) => $"https://api.dicebear.com/7.x/thumbs/svg?seed={user.Id.ToString()}")
            .RuleFor(user => user.CreatedAt, faker => faker.Date.Past(3, DateTime.UtcNow))
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.FirstName, faker => faker.Name.FirstName())
            .RuleFor(user => user.LastName, faker => faker.Name.LastName())
            .RuleFor(user => user.LastLoginLocation, faker =>
            {
                var lon = faker.Random.Double(29, 31);
                var lat = faker.Random.Double(49, 51);
                
                return new Point(lon, lat);
            })
            .RuleFor(user => user.Role, faker => faker.PickRandom<UserRole>())
            .RuleFor(user => user.PasswordHash, faker => faker.Hashids.Encode(faker.Random.Number()));

        var users = Enumerable.Range(0, 2000)
            .Select(_ => usersFaker.Generate())
            .ToArray();
        
        modelBuilder.Entity<UserEntity>().HasData(users);
    }
}