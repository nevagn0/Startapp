using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using MediatR;

namespace Jilo.App.Application.Features.Auth.Logout;

public sealed class LogoutCommandHandler(
    IRefreshTokenRepository repo,
    ITokenHasher tokenHasher)
    : IRequestHandler<LogoutCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await repo.GetRefreshTokenByValue(request.RefreshTokenValue, tokenHasher,
            cancellationToken);

        if (refreshToken.IsError)
        {
            return Unit.Value;
        }

        refreshToken.Value.Revoke();

        return Unit.Value;
    }
}
