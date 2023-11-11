using KittysolomaMap.Api.Dtos;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KittysolomaMap.Api.Rest.Middlewares;

public class ExceptionalMiddleware
{
    private readonly IActionResultExecutor<ObjectResult> _actionResultExecutor;
    private readonly ILogger< ExceptionalMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionalMiddleware(RequestDelegate next,
        ILogger< ExceptionalMiddleware> logger,
        IActionResultExecutor<ObjectResult> actionResultExecutor)
    {
        _next = next;
        _logger = logger;
        _actionResultExecutor = actionResultExecutor;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (BusinessException exception)
        {
            _logger.LogWarning("Exception in middleware message | exception - {Exception}", exception);
            
            var errorDto = new ErrorDto
            {
                Kind = exception.ErrorKind,
                Code = exception.ErrorCode,
                Messages = new List<string>
                {
                    exception.ToString()
                }
            };
            
            await _actionResultExecutor.ExecuteAsync(new ActionContext { HttpContext = context }, new ObjectResult(errorDto)
            {
                StatusCode = errorDto.Kind switch
                {
                    ErrorKind.InvalidData => StatusCodes.Status400BadRequest,
                    ErrorKind.InvalidOperation => StatusCodes.Status409Conflict,
                    ErrorKind.NotFound => StatusCodes.Status404NotFound,
                    ErrorKind.PermissionDenied => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                }
            });
        }
        catch (Exception exception)
        {
            _logger.LogError("Exception in middleware message | exception - {Exception}", exception);
            
            var errorDto = new ErrorDto
            {
                Kind = ErrorKind.Unknown,
                Code = ErrorCode.Unknown,
                Messages = new List<string>
                {
                    exception.ToString()
                }
            };
            
            await _actionResultExecutor.ExecuteAsync(new ActionContext { HttpContext = context },  new ObjectResult(errorDto));
        }
    }
}