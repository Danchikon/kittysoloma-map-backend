namespace KittysolomaMap.Application.Common.Dtos;

public abstract record EntityDtoBase
{
    public required Guid Id { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime? UpdatedAt { get; init; }
}