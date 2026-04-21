using ErrorOr;
using Jilo.App.Api.Authorization;
using Jilo.App.Api.Dto.Lobbies;
using Jilo.App.Application.Features.Invitations.GetOutgoing;
using Jilo.App.Application.Features.Invitations.Send;
using Jilo.App.Application.Features.Lobbies.Create;
using Jilo.App.Application.Features.Lobbies.Delete;
using Jilo.App.Application.Features.Lobbies.Get;
using Jilo.App.Application.Features.Lobbies.GetCurrentLobby;
using Jilo.App.Application.Features.Lobbies.Kick;
using Jilo.App.Application.Features.Lobbies.Leave;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Jilo.App.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/lobby/")]
public sealed class LobbiesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateLobbyCommandResponse>> CreateAsync([FromBody] CreateRequest request, CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new CreateLobbyCommand(profileId, request.GameId);

        var createResult = await mediator.Send(command, cancellationToken);

        return createResult.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => error.Type switch 
            {
                ErrorType.Conflict => Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: $"Unexpected error. Details: {error.Description}"),
            });
    }

    [HttpGet("current")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetCurrentLobbyQueryResponse>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var query = new GetCurrentLobyQuery(profileId);

        var lobby = await mediator.Send(query, cancellationToken);

        return lobby.MatchFirst(
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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetLobbyQueryResponse>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetLobbyQuery(id);

        var lobby = await mediator.Send(query, cancellationToken);

        return lobby.MatchFirst(
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

    [HttpPost("{id:guid}/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> LeaveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new LeaveLobbyCommand(profileId, id);

        var leaveResult = await mediator.Send(command, cancellationToken);

        return leaveResult.MatchFirst<ActionResult>(
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
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description)
            });
    }

    [Authorize(Policy = PolicyNames.LobbyOwner)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteCommand(id);

        var deleteResult = await mediator.Send(command, cancellationToken);

        return deleteResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Ok(),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description),
            });
    }

    [Authorize(Policy = PolicyNames.LobbyOwner)]
    [HttpPost("{id:guid}/kick/{profileId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> KickAsync(Guid id, Guid profileId, CancellationToken cancellationToken = default)
    {
        var command = new KickFromLobbyCommand(id, profileId);

        var kickResult = await mediator.Send(command, cancellationToken);

        return kickResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
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

    [Authorize(Policy = PolicyNames.LobbyOwner)]
    [HttpPost("{id:guid}/invitations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SendInvitationAsync([FromRoute] Guid id, [FromBody] SendInvitationRequest request,
        CancellationToken cancellationToken = default)
    {
        var profileIdStr = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Profile)?.Value;
        if (profileIdStr is null || !Guid.TryParse(profileIdStr, out var profileId))
        {
            return Problem(statusCode: StatusCodes.Status401Unauthorized);
        }

        var command = new SendInvitationCommand(profileId, request.RecieverProfileId, id);

        var sendInvitationResult = await mediator.Send(command, cancellationToken);

        return sendInvitationResult.MatchFirst<ActionResult>(
            onValue: value => Ok(),
            onFirstError: error => error.Type switch
            {
                ErrorType.NotFound => Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Conflict => Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: error.Code,
                    detail: error.Description),
                ErrorType.Failure => Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: error.Code,
                    detail: error.Description),
                _ => Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code,
                    detail: error.Description)
            });
    }

    [Authorize(Policy = PolicyNames.LobbyOwner)]
    [HttpGet("{id:guid}/invitations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetOutgoingInvitationsQueryResponse>> GetOutgoingInvitationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetOutgoingInvitationsQuery(id);

        var invitations = await mediator.Send(query, cancellationToken);

        return invitations.MatchFirst(
            onValue: value => Ok(value),
            onFirstError: error => Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: error.Code,
                detail: $"Unexpected error. Details: {error.Description}"));
    }
}
