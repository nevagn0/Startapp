using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.InvitationEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class InvitationRepository(ServiceContext context) : IInvitationRepository
{
    public void Add(Invitation invitation)
    {
        context.Invitations.Add(invitation);
    }

    public Task<bool> AnyPendingForLobbyForUser(Guid lobbyId, Guid toProfileId, CancellationToken cancellationToken)
    {
        return context.Invitations.AnyAsync(i => i.ReceiverProfileId == toProfileId && i.LobbyId == lobbyId && i.Status == InvitationStatus.Pending, 
            cancellationToken);
    }

    public void Delete(Invitation invitation)
    {
        context.Invitations.Remove(invitation);
    }

    public async Task<ErrorOr<Invitation>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invitation = await context.Invitations.FindAsync([id], cancellationToken);

        if (invitation is null)
        {
            return Errors.Invitation.NotFound;
        }

        return invitation;
    }
}
