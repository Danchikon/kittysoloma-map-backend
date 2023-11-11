using KittysolomaMap.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KittysolomaMap.Api.Rest.Controllers;

[ApiController]           
[Route("api/users")]   
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUserAsync(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }


    [HttpPost("login")]
    public async Task<ActionResult> LoginUserAsync(
        [FromBody] LoginUserCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }
}