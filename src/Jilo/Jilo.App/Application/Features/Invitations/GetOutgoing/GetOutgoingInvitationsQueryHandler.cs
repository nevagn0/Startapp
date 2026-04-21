using ErrorOr;
using Jilo.App.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Application.Features.Invitations.GetOutgoing;

public sealed class GetOutgoingInvitationsQueryHandler(
    ServiceContext context)
    : IRequestHandler<GetOutgoingInvitationsQuery, ErrorOr<GetOutgoingInvitationsQueryResponse>>
{
    public async Task<ErrorOr<GetOutgoingInvitationsQueryResponse>> Handle(GetOutgoingInvitationsQuery request, CancellationToken cancellationToken)
    {
        var invitations = await context.Invitations
            .Where(i => i.LobbyId == request.LobbyId)
            .Select(i => new InvitationDto
            {
                Id = i.Id,
                CreatedAtUtc = i.CreatedAtUtc,
                RecieverUsername = i.ReceiverProfile.Username,
                Status = i.Status.ToString(),
                ToProfileId = i.ReceiverProfileId
            })
            .ToListAsync(cancellationToken);

        return new GetOutgoingInvitationsQueryResponse(invitations);
    }
}
