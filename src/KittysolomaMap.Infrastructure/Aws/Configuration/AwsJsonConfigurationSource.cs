using Amazon;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace KittysolomaMap.Infrastructure.Aws.Configuration;

public class AwsJsonConfigurationSource : JsonConfigurationSource
{
    private readonly RegionEndpoint _region;
    private readonly string _secretId;

    public AwsJsonConfigurationSource(
        RegionEndpoint region, 
        string secretId
        )
    {
        _region = region;
        _secretId = secretId;
    }

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new AwsJsonConfigurationProvider(_secretId, _region, this);
    }
}