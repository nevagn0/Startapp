using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Invitations.Cancel;

public sealed record CancelInvitationCommand(Guid InvitationId, Guid SenderProfileId) : ICommand;
