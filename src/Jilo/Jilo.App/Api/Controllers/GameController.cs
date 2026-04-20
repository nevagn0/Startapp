using ErrorOr;
using Jilo.App.Application.Common.Services;
using Jilo.App.Applicatoin.DTO;
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
    public async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
    {
        var result = await _userGameService.GetAvailableGamesAsync(cancellationToken);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
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

    private IActionResult Problem(List<Error> errors)
    {
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            title: firstError.Code,
            detail: firstError.Description,
            statusCode: statusCode
        );
    }
}