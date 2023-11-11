using KittysolomaMap.Infrastructure.Overpass.Enums;

namespace KittysolomaMap.Infrastructure.Overpass.Dtos;

public record OverpassElementDto
{
    public required long Id { get; init; }
    public required OverpassElementType Type { get; init; }
    public decimal? Lat { get; init; }
    public decimal? Lon { get; init; }
    public ICollection<long>? Nodes { get; init; }
    public ICollection<OverpassMemberDto>? Members { get; init; }
    public Dictionary<string, string> Tags { get; init; } = new ();
}