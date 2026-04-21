using ErrorOr;
using Jilo.App.Application.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface IRefreshTokenRepository
{
    Task<ErrorOr<RefreshToken>> GetRefreshTokenByValue(string value, ITokenHasher tokenHasher, CancellationToken cancellationToken = default);

    void Add(RefreshToken refreshToken);
}
