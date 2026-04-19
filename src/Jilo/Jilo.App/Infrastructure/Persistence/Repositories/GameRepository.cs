using Microsoft.EntityFrameworkCore;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain.GameEntity;
using Jilo.App.Infrastructure.Persistence;

namespace Jilo.App.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ServiceContext _context;

    public GameRepository(ServiceContext context)
    {
        _context = context;
    }

    public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .ToListAsync(cancellationToken);
    }
}