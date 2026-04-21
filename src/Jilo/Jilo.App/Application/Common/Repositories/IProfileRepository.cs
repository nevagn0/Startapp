using ErrorOr;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface IProfileRepository
{
    void Add(Profile profile);

    Task<ErrorOr<Profile>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ErrorOr<Profile>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ErrorOr<Guid>> DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
