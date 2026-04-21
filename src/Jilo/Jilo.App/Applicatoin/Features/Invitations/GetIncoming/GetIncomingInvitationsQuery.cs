using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Invitations.GetIncoming;

public sealed record GetIncomingInvitationsQuery(Guid ProfileId) : IQuery<GetIncomingInvitationsQueryResponse>;
