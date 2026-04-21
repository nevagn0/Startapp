using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Invitations.Accept;

public sealed record AcceptInvitationCommand(Guid InvitationId, Guid ProfileId) : ICommand<AcceptInvitationCommandResponse>;
