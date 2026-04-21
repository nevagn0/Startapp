using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Invitations.GetOutgoing;

public sealed record GetOutgoingInvitationsQuery(Guid LobbyId) : IQuery<GetOutgoingInvitationsQueryResponse>;
