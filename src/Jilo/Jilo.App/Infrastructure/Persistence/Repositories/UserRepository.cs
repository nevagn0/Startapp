using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Domain;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(ServiceContext context) : IUserRepository
{
    public void Add(User user)
    {
        context.Users.Add(user);
    }

    public async Task<(bool EmailExists, bool UsernameExists)> ExistsAsync(string email, string username, CancellationToken cancellationToken = default)
    {
        var existing = await context.Users
            .Where(u => u.Email == email || u.Username == username)
            .Select(u => new { u.Email, u.Username })
            .ToListAsync(cancellationToken);

        return (
            EmailExists: existing.Any(e => e.Email == email),
            UsernameExists: existing.Any(e => e.Username == username)
        );
    }

    public async Task<ErrorOr<User>> FindAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return user;
    }

    public async Task<ErrorOr<User>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
        {
            return Errors.User.NotFound;
        }

        return user;
    }
}
