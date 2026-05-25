using Microsoft.EntityFrameworkCore;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.DTO;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public class PlayerSearchRepository : IPlayerSearchRepository
{
    private readonly ServiceContext _context;

    public PlayerSearchRepository(ServiceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlayerSearchResponse>> SearchPlayersAsync(
        SearchPlayersRequest request,
        CancellationToken cancellationToken = default)
    {

        var query = _context.UserGames
            .Include(ug => ug.Profile)
            .Include(ug => ug.GameCatalog)
            .Where(ug => ug.GameId == request.GameId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Rank))
        {
            query = query.Where(ug => ug.Rank == request.Rank);
        }

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            query = query.Where(ug => ug.Role == request.Role);
        }

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            query = query.Where(ug =>
                ug.Profile.Username.Contains(request.Query) ||
                (ug.Profile.Bio != null && ug.Profile.Bio.Contains(request.Query)));
        }

        var results = await query
            .Select(ug => new PlayerSearchResponse(
                ug.Profile.Id,
                ug.Profile.Username,
                ug.Profile.Bio ?? string.Empty,
                ug.Profile.AvatarUrl,
                ug.Profile.Rating,
                ug.GameCatalog.GameName,
                ug.Role,
                ug.Rank,
                ug.AddedAtUtc
            ))
            .ToListAsync(cancellationToken);

        return results;
    }
}