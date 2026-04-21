using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Invitations.Send;

public sealed record SendInvitationCommand(Guid LobbyOwnerId, Guid ReceiverProfileId, Guid LobbyId)
    : ICommand;
