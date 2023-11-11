using KittysolomaMap.Api.Dtos.Avatars;
using KittysolomaMap.Application.Avatars.Commands;
using KittysolomaMap.Application.Avatars.Queries;
using KittysolomaMap.Application.FileStorage;
using KittysolomaMap.Domain.Common.Errors;
using KittysolomaMap.Domain.Common.Exceptions;
using KittysolomaMap.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KittysolomaMap.Api.Rest.Controllers;

[ApiController]           
[Route("api/avatars")]   
public class AvatarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AvatarsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
  
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserAvatarAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var avatar = await _mediator.Send(new GetUserAvatarQuery { Id = id }, cancellationToken);

        return File(avatar.Stream, avatar.ContentType);
    }
    
    [HttpPut]
    public async Task<ActionResult> UploadUserAvatarAsync(
        [FromForm] UploadAvatarRequestDto dto,
        CancellationToken cancellationToken
    )
    {
        var command = new UploadUserAvatarCommand
        {
            File = new FileDto
            {
                Stream = dto.File.OpenReadStream(),
                ContentType = dto.File.ContentType
            },
            UserId = dto.UserId,
            FileUrlBase = Request.GetDisplayUrl()
        };
        
        return Ok(await _mediator.Send(command, cancellationToken));
    }
}