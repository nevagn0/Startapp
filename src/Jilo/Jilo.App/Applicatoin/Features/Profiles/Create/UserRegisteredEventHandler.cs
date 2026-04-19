using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Features.Auth.Register;
using Jilo.App.Domain.UserEntity;
using MediatR;

namespace Jilo.App.Applicatoin.Features.Profiles.Create;

public sealed class UserRegisteredEventHandler(
    IProfileRepository repo) : INotificationHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var profile = new Profile(notification.UserId, notification.Username);

        repo.Add(profile);
    }
}
