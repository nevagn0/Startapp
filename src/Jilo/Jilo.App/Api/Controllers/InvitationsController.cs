using ErrorOr;
using Jilo.App.Application.Features.Invitations.Accept;
using Jilo.App.Application.Features.Invitations.Cancel;
using Jilo.App.Application.Features.Invitations.Decline;
using Jilo.App.Application.Features.Invitations.GetIncoming;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Jilo.App.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/invitations/")]
public sealed class InvitationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("incoming")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetIncomingInvitationsQueryResponse>> GetIncomingInvitationsAsync(CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var query = new GetIncomingInvitationsQuery(profileId);

        var invitations = await mediator.Send(query, cancellationToken);

        return invitations.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: error.Code,
                detail: $"Unexpected error. Details: {error.Description}"));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CancelInvitationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new CancelInvitationCommand(id, profileId);

        var deleteResult = await mediator.Send(command, cancellationToken);

        return deleteResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
            onFirstError: error => error.Type switch
            {
                ErrorType.Failure => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Forbidden => Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description)
            });
    }

    [HttpPost("{id:guid}/accept")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AcceptInvitationCommandResponse>> AcceptInvitationAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new AcceptInvitationCommand(id, profileId);

        var acceptResult = await mediator.Send(command, cancellationToken);

        return acceptResult.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Failure => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Forbidden => Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Conflict => Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }

    [HttpPost("{id:guid}/decline")]
    public async Task<ActionResult> DeclineInvitationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new DeclineInvitationCommand(id, profileId);

        var declineInvitationResult = await mediator.Send(command, cancellationToken);

        return declineInvitationResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Failure => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Forbidden => Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}")
            });
    }
}
