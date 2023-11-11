using KittysolomaMap.Infrastructure.Overpass.Enums;

namespace KittysolomaMap.Infrastructure.Overpass.Dtos;

public record OverpassMemberDto
{
    public required long Ref { get; init; }
    public required OverpassElementType Type { get; init; }
    public string? Role { get; init; }
}