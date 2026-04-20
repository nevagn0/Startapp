using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Jilo.App.Application.Common.Services;
using Jilo.App.Applicatoin.DTO;

namespace Jilo.App.API.Controllers;

[ApiController]
[Route("api/v1/search/")]
public class PlayerSearchController : ControllerBase
{
    private readonly IPlayerSearchService _playerSearchService;

    public PlayerSearchController(IPlayerSearchService playerSearchService)
    {
        _playerSearchService = playerSearchService;
    }

    [HttpGet("players")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPlayers(
        [FromQuery] Guid gameId,
        [FromQuery] string? rank = null,
        [FromQuery] string? role = null,
        [FromQuery] int? minHours = null,
        [FromQuery] string? query = null,
        CancellationToken cancellationToken = default)
    {
        var request = new SearchPlayersRequest(gameId, rank, role, query);

        var result = await _playerSearchService.SearchPlayersAsync(request, cancellationToken);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    private IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            title: firstError.Code,
            detail: firstError.Description,
            statusCode: statusCode
        );
    }
}