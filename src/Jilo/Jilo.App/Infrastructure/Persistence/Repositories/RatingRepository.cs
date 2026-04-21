using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain.RatingEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class RatingRepository(ServiceContext context) : IRatingRepository
{
    public void Add(Rating rating)
    {
        context.Ratings.Add(rating);
    }

    public async Task<bool> AnyByLobbyAndRaterAndTarget(Guid lobbyId, Guid raterProfileId, Guid targetProfileId,
        CancellationToken cancellationToken = default)
    {
        return await context.Ratings
            .AnyAsync(r =>
                r.LobbyId == lobbyId &&
                r.RaterProfileId == raterProfileId &&
                r.TargetProfileId == targetProfileId,
                cancellationToken);
    }
}
