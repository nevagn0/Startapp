using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.LobbyEntity;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class LobbyMemberRepository(ServiceContext context) : ILobbyMemberRepository
{
    public void Delete(LobbyMember lobbyMember)
    {
        context.LobbyMembers.Remove(lobbyMember);
    }

    public async Task<ErrorOr<LobbyMember>> FindAsync(Guid lobbyId, Guid profileId, CancellationToken cancellationToken = default)
    {
        var lobbyMember = await context.LobbyMembers
            .FindAsync([lobbyId, profileId], cancellationToken);

        if (lobbyMember is null)
        {
            return Errors.LobbyMember.NotFound;
        }

        return lobbyMember;
    }
}
