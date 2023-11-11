using KittysolomaMap.Api.Dtos.Avatars;
using KittysolomaMap.Application.Avatars.Commands;
using KittysolomaMap.Application.Avatars.Queries;
using KittysolomaMap.Application.FileStorage;
using KittysolomaMap.Application.Paths.Queries;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using KittysolomaMap.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KittysolomaMap.Api.Rest.Controllers;

[ApiController]           
[Route("api/paths")]   
public class PathsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PathsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("shortest")]
    public async Task<ActionResult> GetShortestPathAsync([FromQuery] GetShortestPathQuery query, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }
}