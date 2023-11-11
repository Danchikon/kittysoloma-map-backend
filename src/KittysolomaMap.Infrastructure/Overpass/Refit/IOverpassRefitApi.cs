using KittysolomaMap.Infrastructure.Overpass.Dtos;
using Refit;

namespace KittysolomaMap.Infrastructure.Overpass.Refit;

public interface IOverpassRefitApi
{
    [Post("/api/interpreter")]
    Task<ApiResponse<OverpassResponseDto<OverpassElementDto>>> QueryAsync([Body(BodySerializationMethod.Default)] string query, CancellationToken cancellationToken);
}