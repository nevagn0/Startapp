using ErrorOr;
using Jilo.App.Domain.InvitationEntity;
using Jilo.App.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Applicatoin.Features.Invitations.GetIncoming;

public sealed class GetIncomingInvitationsQueryHandler(
    ServiceContext context)
    : IRequestHandler<GetIncomingInvitationsQuery, ErrorOr<GetIncomingInvitationsQueryResponse>>
{
    public async Task<ErrorOr<GetIncomingInvitationsQueryResponse>> Handle(GetIncomingInvitationsQuery request, CancellationToken cancellationToken)
    {
        var invitations = await context.Invitations
            .Where(i => i.Status == InvitationStatus.Pending && i.ReceiverProfileId == request.ProfileId)
            .Select(i => new InvitationDto
            {
                Id = i.Id,
                CreatedAtUtc = i.CreatedAtUtc,
                ExpiresAtUtc = i.ExpiresAtUtc,
                LobbyId = i.LobbyId,
                SenderProfileId = i.SenderProfileId,
                SenderUsername = i.SenderProfile.Username,
                GameId = i.Lobby.Game.Id,
                GameName = i.Lobby.Game.GameName,
                GameCoverImageUrl = i.Lobby.Game.CoverImageUrl
            })
            .ToListAsync(cancellationToken);

        return new GetIncomingInvitationsQueryResponse(invitations);
    }
}
