using ErrorOr;
using Jilo.App.Domain.LobbyEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface ILobbyRepository
{
    Task<ErrorOr<Lobby>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> AnyActiveWithMemberAsync(Guid profileId, CancellationToken cancellationToken = default);

    void Add(Lobby lobby);

    void Delete(Lobby lobby);
}
