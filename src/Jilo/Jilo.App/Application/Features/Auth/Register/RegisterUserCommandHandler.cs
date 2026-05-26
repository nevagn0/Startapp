using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using Jilo.App.Domain;
using Jilo.App.Domain.UserEntity;
using MediatR;

namespace Jilo.App.Application.Features.Auth.Register;

public sealed class RegisterUserCommandHandler(
    IMediator mediator,
    IUserRepository repo,
    IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResponse>>
{
    public async Task<ErrorOr<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await repo.ExistsAsync(request.Username, cancellationToken);

        if (exists)
        {
            return Errors.User.UsernameAlreadyExists;
        }

        var passwordHash = passwordHasher.HashPassword(request.Password);

        var user = new User(request.Username, passwordHash, Role.Player);

        repo.Add(user);

        await mediator.Publish(new UserRegisteredEvent(user.Id, user.Username), cancellationToken);

        return new RegisterUserResponse(
            user.Id,
            user.Username,
            user.CreatedAtUtc,
            user.UpdatedAtUtc);
    }
}
