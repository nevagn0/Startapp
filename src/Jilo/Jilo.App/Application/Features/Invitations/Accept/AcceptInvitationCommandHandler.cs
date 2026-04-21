using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain;
using MediatR;

namespace Jilo.App.Application.Features.Invitations.Accept;

public sealed class AcceptInvitationCommandHandler(
    IInvitationRepository invitationRepo,
    ILobbyRepository lobbyRepo)
    : IRequestHandler<AcceptInvitationCommand, ErrorOr<AcceptInvitationCommandResponse>>
{
    public async Task<ErrorOr<AcceptInvitationCommandResponse>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await invitationRepo.GetAsync(request.InvitationId, cancellationToken);
        if (invitation.IsError)
        {
            return invitation.Errors;
        }

        if (invitation.Value.ReceiverProfileId != request.ProfileId)
        {
            return Errors.Invitation.NotAReciever;
        }

        if (invitation.Value.ExpiresAtUtc <= DateTime.UtcNow)
        {
            var setExpiredResult = invitation.Value.Expired();

            if (setExpiredResult.IsError)
            {
                return setExpiredResult.Errors;
            }

            return Errors.Invitation.Expired;
        }

        var lobby = await lobbyRepo.GetAsync(invitation.Value.LobbyId, cancellationToken);
        if (lobby.IsError)
        {
            return lobby.Errors;
        }

        if (lobby.Value.Members.Count == 5)
        {
            return Errors.Lobby.Full;
        }

        var addMemberResult = lobby.Value.AddMember(invitation.Value.ReceiverProfileId);
        if (addMemberResult.IsError)
        {
            return addMemberResult.Errors;
        }

        var acceptInvitationResult = invitation.Value.Accept();
        if (acceptInvitationResult.IsError)
        {
            return acceptInvitationResult.Errors;
        }

        return new AcceptInvitationCommandResponse(lobby.Value.Id);
    }
}
