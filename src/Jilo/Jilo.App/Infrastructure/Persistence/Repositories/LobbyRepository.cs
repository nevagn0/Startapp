using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.LobbyEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class LobbyRepository(ServiceContext context) : ILobbyRepository
{
    public void Add(Lobby lobby)
    {
        context.Lobbies.Add(lobby);
    }

    public Task<bool> AnyActiveWithMemberAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        return context.Lobbies
            .AnyAsync(l => l.Members
                .Any(m => m.ProfileId == profileId), cancellationToken);
    }

    public Task<bool> AnyActiveWithOwnerAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        return context.Lobbies.AnyAsync(l => l.CreatedByProfileId == profileId, cancellationToken);
    }

    public void Delete(Lobby lobby)
    {
        context.Lobbies.Remove(lobby);
    }

    public async Task<ErrorOr<Lobby>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lobby = await context.Lobbies
            .Include(l => l.Members)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (lobby is null)
        {
            return Errors.Lobby.NotFound;
        }

        return lobby;
    }
}
