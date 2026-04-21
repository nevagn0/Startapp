using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Invitations.Decline;

public sealed record DeclineInvitationCommand(Guid InvitationId, Guid RecieverProfileId) : ICommand;
