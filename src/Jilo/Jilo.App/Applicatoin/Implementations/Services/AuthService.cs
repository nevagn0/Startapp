using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Applicatoin.Dto.RegisterUser;
using Jilo.App.Domain;
using Jilo.App.Infrastructure.Persistence;

namespace Jilo.App.Applicatoin.Implementations.Services;

public sealed class AuthService(
    IPasswordHasher passwordHasher,
    ServiceContext context)
    : IAuthService
{
    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var passwordHash = passwordHasher.HashPassword(request.Password);

        var user = new User(request.Username, request.Email, passwordHash, Role.Player);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
    
        return new RegisterUserResponse(user.Id, user.Email, user.Username, user.CreatedAtUtc, user.UpdatedAtUtc);
    }
}
