namespace KittysolomaMap.Infrastructure.Overpass;

public record OverpassOptions
{
    public const string Section = "Overpass";
    
    public required string Url { get; init; }
}