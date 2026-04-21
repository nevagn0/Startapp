namespace Jilo.App.Application.Features.Invitations.GetIncoming;

public sealed record GetIncomingInvitationsQueryResponse(IReadOnlyCollection<InvitationDto> Invitations);
