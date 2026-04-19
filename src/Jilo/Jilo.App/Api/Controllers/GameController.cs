using ErrorOr;
using Jilo.App.Application.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jilo.App.API.Controllers;

[ApiController]
[Route("api/user/games")]
[Authorize]
public class GameController : ControllerBase
{
    private readonly IUserGameService _userGameService;

    public GameController(IUserGameService userGameService)
    {
        _userGameService = userGameService;
    }

    [HttpGet("games")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllGames(CancellationToken cancellationToken)
    {
        var result = await _userGameService.GetAvailableGamesAsync(cancellationToken);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }


    [HttpGet("UserGame")]
    public async Task<IActionResult> GetMyGames(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _userGameService.GetUserGamesAsync(userId, cancellationToken);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst("sub")?.Value
                          ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");

        return Guid.Parse(userIdClaim);
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