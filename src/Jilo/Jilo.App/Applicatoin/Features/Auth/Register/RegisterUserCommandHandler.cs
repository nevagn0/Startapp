using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.UserEntity;
using MediatR;

namespace Jilo.App.Applicatoin.Features.Auth.Register;

public sealed class RegisterUserCommandHandler(
    IUserRepository repo,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResponse>>
{
    public async Task<ErrorOr<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var (EmailExists, UsernameExists) = await repo.ExistsAsync(request.Email, request.Username, cancellationToken);

        if (UsernameExists && UsernameExists)
        {
            return Errors.User.EmailAndUsernameAlreadyExist;
        }

        if (UsernameExists)
        {
            return Errors.User.UsernameAlreadyExists;
        }

        if (EmailExists)
        {
            return Errors.User.EmailAlreadyExists;
        }

        var passwordHash = passwordHasher.HashPassword(request.Password);

        var user = new User(request.Username, request.Email, passwordHash, Role.Player);

        repo.Add(user);

        return new RegisterUserResponse(
            user.Id,
            user.Email,
            user.Username,
            user.CreatedAtUtc,
            user.UpdatedAtUtc);
    }
}
