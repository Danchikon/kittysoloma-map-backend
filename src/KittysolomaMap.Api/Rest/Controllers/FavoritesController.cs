using KittysolomaMap.Application.Favorites.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KittysolomaMap.Api.Rest.Controllers;

[ApiController]           
[Route("api/favorites")]   
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateFavoriteAsync(
        [FromBody] CreateFavoriteCommand command,
        CancellationToken cancellationToken
    )
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }
}