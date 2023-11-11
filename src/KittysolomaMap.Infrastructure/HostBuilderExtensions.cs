using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace KittysolomaMap.Infrastructure;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSerilogLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((context, serviceProvider, configuration) => {
            configuration.ReadFrom.Configuration(context.Configuration);
            configuration.ReadFrom.Services(serviceProvider);
            configuration.Enrich.WithExceptionDetails();
            configuration.Enrich.FromLogContext();
            configuration.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}][{Level:u3}][{SourceContext}] {Message:lj}{Exception}{NewLine}");
        });

        return builder;
    }
}