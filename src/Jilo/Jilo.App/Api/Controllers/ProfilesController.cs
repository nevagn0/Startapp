using ErrorOr;
using Jilo.App.Api.Authorization;
using Jilo.App.Api.Dto.Profiles;
using Jilo.App.Application.Common.Services;
using Jilo.App.Applicatoin.Features.Profiles.Get;
using Jilo.App.Applicatoin.Features.Profiles.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jilo.App.Api.Controllers;

[ApiController]
[Route("api/v1/profile/")]
public sealed class ProfilesController(IMediator mediator, IUserGameService userGameService) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IUserGameService _userGameService = userGameService;

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetProfileQueryResponse>> GetMyAsync(CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var query = new GetProfileQuery(userId);

        var profile = await _mediator.Send(query, cancellationToken);

        return profile.MatchFirst(
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
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetProfileQueryResponse>> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = new GetProfileQuery(userId);

        var profile = await _mediator.Send(query, cancellationToken);

        return profile.MatchFirst(
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
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [Authorize(Policy = PolicyNames.ProfileOwner)]
    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UpdateProfileCommandResponse>> UpdateAsync([FromBody] UpdateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdStr is null || !Guid.TryParse(userIdStr, out var userId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new UpdateProfileCommand(userId, request.Bio);

        var updateResult = await _mediator.Send(command, cancellationToken);

        return updateResult.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: error.Code,
                    detail: $"Possibly you're trying to change another person's profile. Detail: {error.Description}"),
                ErrorType.Validation => ValidationProblem(new ValidationProblemDetails(
                    updateResult.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray()))),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [Authorize]
    [HttpGet("games")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMyGames(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _userGameService.GetUserGamesAsync(userId, cancellationToken);

        return result.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    [Authorize]
    [HttpPost("addgames")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddGameToUser([FromBody] AddGameToUserRequest request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _userGameService.AddGameToUserAsync(userId, request, cancellationToken);

        return result.Match(
            _ => Ok(new { message = "Game added successfully" }),
            errors => Problem(errors)
        );
    }

    [Authorize]
    [HttpPut("updategames/{userGameId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [Authorize]
    [HttpDelete("deletegames/{userGameId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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
            statusCode: statusCode,
            title: firstError.Code,
            detail: firstError.Description
        );
    }
}