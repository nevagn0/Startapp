using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.InvitationEntity;
using MediatR;

namespace Jilo.App.Applicatoin.Features.Invitations.Send;

public sealed class SendInvitationCommandHandler(
    ILobbyRepository lobbyRepo,
    IInvitationRepository invitationRepo)
    : IRequestHandler<SendInvitationCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
    {
        var lobby = await lobbyRepo.GetAsync(request.LobbyId, cancellationToken);
        if (lobby.IsError)
        {
            return lobby.Errors;
        }

        bool alreadyHaveInvitationToThisLobby = await invitationRepo
            .AnyPendingForLobbyForUser(lobby.Value.Id, request.ReceiverProfileId, cancellationToken);
        if (alreadyHaveInvitationToThisLobby)
        {
            return Errors.Lobby.PendingInvitation;
        }

        if (lobby.Value.Members.Any(m => m.ProfileId == request.ReceiverProfileId))
        {
            return Errors.Lobby.AlreadyInLobby;
        }

        if (lobby.Value.Members.Count == 5)
        {
            return Errors.Lobby.Full;
        }

        var invitation = new Invitation(request.ReceiverProfileId, request.LobbyOwnerId, request.LobbyId);

        invitationRepo.Add(invitation);

        return Unit.Value;
    }
}
