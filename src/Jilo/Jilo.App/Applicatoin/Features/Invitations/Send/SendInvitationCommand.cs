using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Invitations.Send;

public sealed record SendInvitationCommand(Guid LobbyOwnerId, Guid ReceiverProfileId, Guid LobbyId)
    : ICommand;
