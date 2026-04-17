using ErrorOr;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Domain;

namespace Jilo.App.Applicatoin.Common.Repositories;

public interface IRefreshTokenRepository
{
    Task<ErrorOr<RefreshToken>> GetRefreshTokenByValue(string value, ITokenHasher tokenHasher, CancellationToken cancellationToken = default);

    void Add(RefreshToken refreshToken);
}
