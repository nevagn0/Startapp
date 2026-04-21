using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Invitations.Cancel;

public sealed record CancelInvitationCommand(Guid InvitationId, Guid SenderProfileId) : ICommand;
