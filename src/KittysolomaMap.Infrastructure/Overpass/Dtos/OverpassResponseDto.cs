namespace KittysolomaMap.Infrastructure.Overpass.Dtos;

public record OverpassResponseDto<TElement>
{
    public required decimal Version { get; init; }
    public required string Generator { get; init; }
    public ICollection<TElement> Elements { get; init; } = Array.Empty<TElement>();
}