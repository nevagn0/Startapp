using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository(ServiceContext context) : IRefreshTokenRepository
{
    public void Add(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
    }

    public async Task<ErrorOr<RefreshToken>> GetRefreshTokenByValue(string refreshTokenValue, ITokenHasher tokenHasher,
        CancellationToken cancellationToken = default)
    {
        var tokenHash = tokenHasher.HashToken(refreshTokenValue);

        var refreshToken = await context.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);

        if (refreshToken is null)
        {
            return Errors.RefreshToken.NotFound;
        }

        return refreshToken;
    }
}
