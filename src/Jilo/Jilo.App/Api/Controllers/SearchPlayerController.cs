using ErrorOr;
using Jilo.App.Application.Common.Services;
using Jilo.App.Application.DTO;
using Microsoft.AspNetCore.Mvc;

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
        [FromQuery] string? query = null,
        CancellationToken cancellationToken = default)
    {
        var request = new SearchPlayersRequest(gameId, rank, role, query);

        var searchResult = await _playerSearchService.SearchPlayersAsync(request, cancellationToken);

        return searchResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}"),
                ErrorType.Validation => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}"),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }
}