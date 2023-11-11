namespace KittysolomaMap.Domain.Filtering.Nodes;

public record NodesFilterOptions 
{
    public int? Limit { get; init; }
    public string? NameUk { get; init; }
    public string? NameEn { get; init; }
    public bool? BarrierFree { get; init; }
    public NodesTagsFilterOptions? Tags { get; init; }
};