using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Features.Ratings.RateUser;
using MediatR;

namespace Jilo.App.Application.Features.Profiles.UpdateRating;

public sealed class UpdateRatingEventHandler(
    IProfileRepository repo)
    : INotificationHandler<UserRatedEvent>
{
    public async Task Handle(UserRatedEvent notification, CancellationToken cancellationToken)
    {
        var profile = await repo.GetAsync(notification.RatedProfileId, cancellationToken);

        if (profile.IsError)
        {
            return;
        }

        profile.Value.UpdateRating(notification.Value);
    }
}
