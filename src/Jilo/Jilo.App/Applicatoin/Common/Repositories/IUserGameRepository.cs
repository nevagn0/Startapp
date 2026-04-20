using Jilo.App.Domain.Entities;

namespace Jilo.App.Application.Common.Repositories;

public interface IUserGameRepository
{
    Task AddAsync(UserGame userGame, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserGame userGame, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserGame?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserGame>> GetByProfileIdAsync(Guid userId, CancellationToken cancellationToken = default);
}