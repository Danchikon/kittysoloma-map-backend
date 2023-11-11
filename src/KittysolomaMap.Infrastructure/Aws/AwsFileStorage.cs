using Amazon.S3;
using Amazon.S3.Model;
using KittysolomaMap.Application.Dtos;
using KittysolomaMap.Application.FileStorage;

namespace KittysolomaMap.Infrastructure.Aws;

public class AwsFileStorage : IFileStorage
{
    private readonly IAmazonS3 _amazonS3;

    public AwsFileStorage(IAmazonS3 amazonS3)
    {
        _amazonS3 = amazonS3;
    }

    public string GeneratePreSignedUrl(
        string folder, 
        string name, 
        TimeSpan duration
        )
    {
        return GeneratePreSignedUrl(folder, name, DateTime.UtcNow.Add(duration));
    }

    public string GeneratePreSignedUrl(
        string folder,
        string name, 
        DateTime expirationDate
        )
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = folder,
            Key = name,
            Expires = expirationDate
        };

        return _amazonS3.GetPreSignedURL(request);
    }

    public async Task UploadAsync(
        string folder,
        string name,
        FileDto file,
        CancellationToken cancellationToken
        )
    {
        var request = new PutObjectRequest
        {
            BucketName = folder,
            Key = name,
            InputStream = file.Stream,
            ContentType = file.ContentType,
        };

        await _amazonS3.PutObjectAsync(request, cancellationToken);
    }

    public async Task<FileDto> DownloadAsync(
        string folder,
        string name, 
        CancellationToken cancellationToken
        )
    {
        var request = new GetObjectRequest
        {
            BucketName = folder,
            Key = name,
        };

        var response = await _amazonS3.GetObjectAsync(request, cancellationToken);

        return new FileDto
        {
            Stream =  response.ResponseStream,
            ContentType = response.Headers.ContentType
        };
    }

    public async Task RemoveAsync(string folder, string name, CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = folder,
            Key = name
        };

        await _amazonS3.DeleteObjectAsync(request, cancellationToken);
    }

    public async Task CopyAsync(
        string sourceFolder,
        string sourceName,
        string destinationFolder,
        string destinationName,
        CancellationToken cancellationToken
        )
    {
        var request = new CopyObjectRequest
        {
            DestinationBucket = destinationFolder,
            DestinationKey = destinationName,
            SourceBucket = sourceFolder,
            SourceKey = sourceName
        };
        
        await _amazonS3.CopyObjectAsync(request, cancellationToken);
    }
}