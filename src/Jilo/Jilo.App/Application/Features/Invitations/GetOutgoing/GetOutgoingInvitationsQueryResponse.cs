namespace Jilo.App.Application.Features.Invitations.GetOutgoing;

public sealed record GetOutgoingInvitationsQueryResponse(IReadOnlyList<InvitationDto> Invitations);
