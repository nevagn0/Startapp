using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Invitations.GetOutgoing;

public sealed record GetOutgoingInvitationsQuery(Guid LobbyId) : IQuery<GetOutgoingInvitationsQueryResponse>;
