using KittysolomaMap.Application.Common.Dtos;


namespace KittysolomaMap.Application.Dtos;

public record WayTagDto : EntityDtoBase
{
    public required long WayId { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
}