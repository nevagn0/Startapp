using Jilo.App.Domain.RatingEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface IRatingRepository
{
    void Add(Rating rating);

    Task<bool> AnyByLobbyAndRaterAndTarget(Guid lobbyId, Guid raterProfileId, Guid targerProfileId,
        CancellationToken cancellationToken = default);
}
