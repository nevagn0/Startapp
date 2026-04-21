using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Invitations.GetIncoming;

public sealed record GetIncomingInvitationsQuery(Guid ProfileId) : IQuery<GetIncomingInvitationsQueryResponse>;
