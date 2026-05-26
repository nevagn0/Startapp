using ErrorOr;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface IUserRepository
{
    void Add(User user);

    Task<bool> ExistsAsync(string username, CancellationToken cancellationToken = default);

    Task<ErrorOr<User>> FindAsync(string username, CancellationToken cancellationToken = default);

    Task<ErrorOr<User>> GetAsync(Guid Id, CancellationToken cancellationToken = default);
}
