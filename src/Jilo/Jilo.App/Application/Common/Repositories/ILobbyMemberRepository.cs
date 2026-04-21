using ErrorOr;
using Jilo.App.Domain.LobbyEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface ILobbyMemberRepository
{
    Task<ErrorOr<LobbyMember>> FindAsync(Guid lobbyId, Guid profileId, CancellationToken cancellationToken = default);

    void Delete(LobbyMember lobbyMember);
}
