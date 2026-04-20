using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.Models;
using Jilo.App.Domain.UserEntity;
using MediatR;

namespace Jilo.App.Applicatoin.Features.Auth.Refresh;

public sealed class RefreshCommandHandler(
    ITokenHasher tokenHasher,
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository)
    : IRequestHandler<RefreshCommand, ErrorOr<TokenPair>>
{
    public async Task<ErrorOr<TokenPair>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetRefreshTokenByValue(request.RefreshTokenValue,
            tokenHasher, cancellationToken);
        if (refreshToken.IsError)
        {
            return Errors.Auth.Unauthorized;
        }

        if (!refreshToken.Value.IsActive)
        {
            return Errors.Auth.Unauthorized;
        }

        var user = await userRepository.GetAsync(refreshToken.Value.UserId, cancellationToken);
        if (user.IsError)
        {
            return Errors.Auth.Unauthorized;
        }

        refreshToken.Value.Revoke();

        var tokenPair = await tokenProvider.GetTokensForUser(user.Value);
        if (tokenPair.IsError)
        {
            return tokenPair.Errors;
        }

        var tokenHash = tokenHasher.HashToken(tokenPair.Value.RefreshToken);

        var newRefreshToken = new RefreshToken(user.Value.Id, tokenHash, DateTime.UtcNow.AddDays(30));

        refreshTokenRepository.Add(newRefreshToken);

        return tokenPair.Value;
    }
}
