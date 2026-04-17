using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.Models;
using MediatR;

namespace Jilo.App.Applicatoin.Features.Auth.Login;

public sealed class LoginUserCommandHandler(
    IUserRepository userRepo,
    IRefreshTokenRepository refreshTokenRepo,
    IPasswordHasher passwordHasher,
    ITokenHasher tokenHasher,
    ITokenProvider tokenProvider)
    : IRequestHandler<LoginUserCommand, ErrorOr<TokenPair>>
{
    public async Task<ErrorOr<TokenPair>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.FindAsync(request.Email, cancellationToken);

        if (user.IsError)
        {
            return Errors.Auth.Unauthorized;
        }

        if (!passwordHasher.VerifyPassword(request.Password, user.Value.PasswordHash))
        {
            return Errors.Auth.Unauthorized;
        }

        var tokenPair = tokenProvider.GetTokensForUser(user.Value);

        if (tokenPair.IsError)
        {
            return tokenPair.Errors;
        }

        var tokenHash = tokenHasher.HashToken(tokenPair.Value.RefreshToken);

        var refreshToken = new RefreshToken(user.Value.Id, tokenHash, DateTime.UtcNow.AddDays(30));

        refreshTokenRepo.Add(refreshToken);

        return tokenPair.Value;
    }
}
