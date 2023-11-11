using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration.Json;

namespace KittysolomaMap.Infrastructure.Aws.Configuration;

public class AwsJsonConfigurationProvider : JsonConfigurationProvider
{
    private readonly RegionEndpoint _region;
    private readonly string _secretId;
    
    public AwsJsonConfigurationProvider(
        string secretId, 
        RegionEndpoint region,
        AwsJsonConfigurationSource awsJsonConfigurationSource
        ) : base(awsJsonConfigurationSource)
    {
        _secretId = secretId;
        _region = region;
    }

    public override void Load()
    {
        Load(GetSecret(_secretId));
    }

    private MemoryStream GetSecret(string secretId)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = secretId
        };

        using var client = new AmazonSecretsManagerClient(_region);
        
        var response = client.GetSecretValueAsync(request).Result;
        
        if (response.SecretString is not null)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(response.SecretString));
        }
        
        var memoryStream = response.SecretBinary;
        using var reader = new StreamReader(memoryStream);
    
        return new MemoryStream(Convert.FromBase64String(reader.ReadToEnd()));
    }
}