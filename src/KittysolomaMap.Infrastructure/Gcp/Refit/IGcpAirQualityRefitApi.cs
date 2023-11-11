using KittysolomaMap.Infrastructure.Gcp.Dtos;
using Refit;

namespace KittysolomaMap.Infrastructure.Gcp.Refit;

public interface IGcpAirQualityRefitApi
{
    [Post("/v1/currentConditions:lookup")]
    Task<ApiResponse<GcpAirQualityDto>> GetAsync(
        [Query] string key, 
        [Body] GetGcpAirQualityRequestDto request,
        CancellationToken cancellationToken
        );
}