namespace KittysolomaMap.Application.FileStorage;

public record FileDto : IAsyncDisposable, IDisposable
{
    public const string DefaultContentType = "application/octet-stream";
    
    public required Stream Stream { get; init; }
    public string ContentType { get; init; } = DefaultContentType;
    
    public long Length => Stream.Length;

    public void Dispose()
    {
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Stream.DisposeAsync();
    }
}