namespace Jilo.App.Applicatoin.Features.Invitations.GetIncoming;

public sealed record GetIncomingInvitationsQueryResponse(IReadOnlyCollection<InvitationDto> Invitations);
