using KittysolomaMap.Domain.Filtering.Nodes;
using KittysolomaMap.Domain.Geo;
using KittysolomaMap.Domain.Common.Pagination;

namespace KittysolomaMap.Domain.Repositories;

public interface INodesRepository
{
    Task<ICollection<NodeEntity>> ToCollectionAsync(
        NodesFilterOptions? filter,
        CancellationToken cancellationToken
    );
}