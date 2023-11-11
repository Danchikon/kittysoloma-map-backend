
namespace KittysolomaMap.Domain.Filtering.Nodes;

public record NodesTagsFilterOptions
{
    public bool? Any { get; init; }
    public NodesTagsSomeSubFilterOptions? Some { get; init; }
}