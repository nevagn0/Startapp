namespace Jilo.App.Applicatoin.Features.Invitations.GetOutgoing;

public sealed record GetOutgoingInvitationsQueryResponse(IReadOnlyList<InvitationDto> Invitations);
