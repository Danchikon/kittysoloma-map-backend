using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace KittysolomaMap.Infrastructure;

public static class HostExtensions
{
    public static async Task<IHost> MigrateAsync<TDbContext>(this IHost host, CancellationToken cancellationToken) where TDbContext: DbContext
    {
        await using var scope = host.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
        
        if (dbContext.Database.GetDbConnection() is not NpgsqlConnection npgsqlConnection)
        {
            return host;
        }
            
        await npgsqlConnection.OpenAsync(cancellationToken);
            
        try
        {
            await npgsqlConnection.ReloadTypesAsync();
        }
        finally
        {
            await npgsqlConnection.CloseAsync();
        }

        return host;
    }
    
    public static async Task<IHost> AwsS3PutBucketsAsync(
        this IHost host,
        IEnumerable<string> buckets, 
        CancellationToken cancellationToken
        )
    {
        await using var scope = host.Services.CreateAsyncScope();
        var amazonS3 = scope.ServiceProvider.GetRequiredService<IAmazonS3>();
        
        var existedBuckets = await amazonS3.ListBucketsAsync(cancellationToken);

        var bucketsToPut = buckets.Except(existedBuckets.Buckets.Select(existed => existed.BucketName));
        var putBucketRequests = bucketsToPut.Select(bucket => new PutBucketRequest
        {
            CannedACL = S3CannedACL.Private,
            BucketName = bucket
        });
        var putBucketTasks = putBucketRequests.Select(request => amazonS3.PutBucketAsync(request, cancellationToken));

        await Task.WhenAll(putBucketTasks);

        return host;
    }
}