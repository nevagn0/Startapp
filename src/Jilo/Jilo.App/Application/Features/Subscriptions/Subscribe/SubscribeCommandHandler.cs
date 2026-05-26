using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.Models;
using Jilo.App.Domain.UserEntity;
using MediatR;

namespace Jilo.App.Application.Features.Subscriptions.Subscribe;

public sealed class SubscribeCommandHandler(
    IProfileRepository profileRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    ITokenHasher tokenHasher
    )
    : IRequestHandler<SubscribeCommand, ErrorOr<TokenPair>>
{
    public async Task<ErrorOr<TokenPair>> Handle(SubscribeCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await profileRepository.GetAsync(request.ProfileId, cancellationToken);
        if (userProfile.IsError)
        {
            return userProfile.Errors;
        }

        var subscribeResult = userProfile.Value.Subscribe();
        if (subscribeResult.IsError)
        {
            return subscribeResult.Errors;
        }

        var refreshToken = await refreshTokenRepository.GetRefreshTokenByValue(request.RefreshTokenValue, tokenHasher, cancellationToken);
        if (refreshToken.IsError)
        {
            return Errors.Auth.Unauthorized;
        }

        if (!refreshToken.Value.IsActive)
        {
            return Errors.Auth.Unauthorized;
        }

        refreshToken.Value.Revoke();

        var user = await userRepository.GetAsync(refreshToken.Value.UserId, cancellationToken);
        if (user.IsError)
        {
            return Errors.Auth.Unauthorized;
        }

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
