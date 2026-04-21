using ErrorOr;
using Jilo.App.Application.Common.Services;
using Jilo.App.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Jilo.App.API.Controllers;

[ApiController]
[Route("api/v1/games/")]
public class GameController : ControllerBase
{
    private readonly IUserGameService _userGameService;

    public GameController(IUserGameService userGameService)
    {
        _userGameService = userGameService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AvailableGameResponse>>> ListAsync(CancellationToken cancellationToken)
    {
        var availableGames = await _userGameService.GetAvailableGamesAsync(cancellationToken);

        return availableGames.MatchFirst<ActionResult>(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GameDto>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var game = await _userGameService.GetGameAsync(id, cancellationToken);

        return game.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description)
            } 
        );
    }
}