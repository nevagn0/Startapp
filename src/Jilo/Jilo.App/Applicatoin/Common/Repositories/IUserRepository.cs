using ErrorOr;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Applicatoin.Common.Repositories;

public interface IUserRepository
{
    void Add(User user);

    Task<(bool EmailExists, bool UsernameExists)> ExistsAsync(string email, string username, CancellationToken cancellationToken = default);

    Task<ErrorOr<User>> FindAsync(string username, CancellationToken cancellationToken = default);

    Task<ErrorOr<User>> GetAsync(Guid Id, CancellationToken cancellationToken = default);
}
