using Microsoft.EntityFrameworkCore;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain.Entities;
using Jilo.App.Infrastructure.Persistence;

namespace Jilo.App.Infrastructure.Repositories;

public class UserGameRepository : IUserGameRepository
{
    private readonly ServiceContext _context;

    public UserGameRepository(ServiceContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserGame userGame, CancellationToken cancellationToken = default)
    {
        await _context.UserGames.AddAsync(userGame, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserGame userGame, CancellationToken cancellationToken = default)
    {
        _context.UserGames.Update(userGame);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userGame = await GetByIdAsync(id, cancellationToken);
        if (userGame != null)
        {
            _context.UserGames.Remove(userGame);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<UserGame?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserGames
            .Include(u => u.GameCatalog)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<UserGame>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserGames
            .Include(u => u.GameCatalog)
            .Where(u => u.ProfileId == userId)
            .ToListAsync(cancellationToken);
    }

}