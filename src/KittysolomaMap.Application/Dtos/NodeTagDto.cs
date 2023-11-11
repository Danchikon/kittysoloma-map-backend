using KittysolomaMap.Application.Common.Dtos;


namespace KittysolomaMap.Application.Dtos;

public record NodeTagDto : EntityDtoBase
{
    public required long NodeId { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
}