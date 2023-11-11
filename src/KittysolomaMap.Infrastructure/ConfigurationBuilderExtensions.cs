using Amazon;
using KittysolomaMap.Infrastructure.Aws;
using KittysolomaMap.Infrastructure.Aws.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace KittysolomaMap.Infrastructure;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAwsConfiguration(
        this IConfigurationBuilder configurationBuilder, 
        IConfiguration configuration,
        IHostEnvironment environment
        )
    {
        // if (environment.IsDevelopment() is false)
        // {
        //     var awsSecretsManagerOptions = configuration
        //         .GetRequiredSection(AwsSecretsManagerOptions.Section)
        //         .Get<AwsSecretsManagerOptions>();
        //     
        //     if (awsSecretsManagerOptions is null)
        //     {
        //         throw new InvalidOperationException($"Section {AwsSecretsManagerOptions.Section} is null");
        //     }
        //     
        //     configurationBuilder.Add(new AwsJsonConfigurationSource(
        //         RegionEndpoint.GetBySystemName(awsSecretsManagerOptions.Region), 
        //         awsSecretsManagerOptions.SecretId
        //         ));
        // }
        
        return configurationBuilder;
    }
}