namespace KittysolomaMap.Domain.Filtering.Nodes;

public record NodesTagsSomeSubFilterOptions
{
    public string? KeyEquals { get; init; }
    public string? ValueEquals { get; init; }
}
