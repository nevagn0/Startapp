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

    [HttpPost("AddGame")]
    public async Task<IActionResult> AddGameToUser([FromBody] AddGameToUserRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _userGameService.AddGameToUserAsync(userId, request, cancellationToken);

        return result.Match(
            _ => Ok(new { message = "Game added successfully" }),
            errors => Problem(errors)
        );
    }

    [HttpPut("{userGameId:guid} GameUpdate")]
    public async Task<IActionResult> UpdateUserGame(
        Guid userGameId,
        [FromBody] UpdateUserGameRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _userGameService.UpdateUserGameAsync(userId, userGameId, request, cancellationToken);

        return result.Match(
            _ => Ok(new { message = "Game updated successfully" }),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{userGameId:guid} GameDelete")]
    public async Task<IActionResult> DeleteUserGame(Guid userGameId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _userGameService.DeleteUserGameAsync(userId, userGameId, cancellationToken);

        return result.Match(
            _ => NoContent(),
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