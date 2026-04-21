using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Invitations.Accept;

public sealed record AcceptInvitationCommand(Guid InvitationId, Guid ProfileId) : ICommand<AcceptInvitationCommandResponse>;
