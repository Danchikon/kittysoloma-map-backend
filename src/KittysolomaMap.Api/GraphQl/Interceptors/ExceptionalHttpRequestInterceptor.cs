using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace KittysolomaMap.Api.GraphQl.Interceptors;

public class ExceptionalHttpRequestInterceptor : DefaultHttpRequestInterceptor
{
    private readonly ILogger<ExceptionalHttpRequestInterceptor> _logger;

    public ExceptionalHttpRequestInterceptor(ILogger<ExceptionalHttpRequestInterceptor> logger)
    {
        _logger = logger;
    }
    
    public override async ValueTask OnCreateAsync(
        HttpContext context,
        IRequestExecutor requestExecutor,
        IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken
        )
    {
        try
        {
            await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError("Creating graphql http request failed | exception - {Exception}", exception);
            throw;
        }
    }
}