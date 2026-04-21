using MediatR;

namespace Jilo.App.Application.Features.Ratings.RateUser;

public sealed record UserRatedEvent(Guid RatedProfileId, int Value) : INotification;
