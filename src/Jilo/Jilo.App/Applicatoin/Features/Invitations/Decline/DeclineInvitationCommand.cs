using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Invitations.Decline;

public sealed record DeclineInvitationCommand(Guid InvitationId, Guid RecieverProfileId) : ICommand;
