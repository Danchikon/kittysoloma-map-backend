using Amazon;
using KittysolomaMap.Api;
using KittysolomaMap.Api.Rest.Middlewares;
using KittysolomaMap.Application;
using KittysolomaMap.Application.FileStorage;
using KittysolomaMap.Infrastructure;
using KittysolomaMap.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var host = builder.Host;
var environment = builder.Environment;
var configuration = builder.Configuration;
var services = builder.Services;

host.UseSerilogLogging();

configuration.AddEnvironmentVariables();
configuration.AddAwsConfiguration(configuration, environment);

services.AddInfrastructure(configuration, environment);
services.AddApplication();
services.AddApi(environment);

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    Task.WhenAll(
            app.MigrateAsync<KittysolomaMapDbContext>(app.Lifetime.ApplicationStopping),
            app.AwsS3PutBucketsAsync(new [] { FoldersNames.AvatarFolder }, app.Lifetime.ApplicationStopping)
            )
        .Wait();
});

app.UseMiddleware<ExceptionalMiddleware>();

app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyHeader();
    corsPolicyBuilder.AllowAnyOrigin();
    corsPolicyBuilder.AllowAnyMethod();
});

app.UseRouting();

if (environment.IsProduction() is false)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL();
app.MapControllers();

await app.RunAsync();