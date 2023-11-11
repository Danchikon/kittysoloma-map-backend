using KittysolomaMap.Application.Common.Dtos;


namespace KittysolomaMap.Application.Dtos;

public record RelationTagDto : EntityDtoBase
{
    public required long RelationId { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
}