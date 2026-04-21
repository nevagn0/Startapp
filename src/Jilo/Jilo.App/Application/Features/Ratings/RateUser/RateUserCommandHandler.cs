using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.RatingEntity;
using MediatR;

namespace Jilo.App.Application.Features.Ratings.RateUser;

public sealed class RateUserCommandHandler(
    ILobbyRepository lobbyRepo,
    IRatingRepository ratingRepo,
    IMediator mediator)
    : IRequestHandler<RateUserCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(RateUserCommand request, CancellationToken cancellationToken)
    {
        var lobby = await lobbyRepo.GetAsync(request.LobbyId, cancellationToken);
        if (lobby.IsError) 
        { 
            return lobby.Errors;
        }

        if (!lobby.Value.Members.Any(m => m.ProfileId == request.RaterProfileId))
        { 
            return Errors.Lobby.NotAMemberOfLobby;
        }

        if (request.RaterProfileId == request.TargetProfileId)
        {
            return Errors.Ratings.CannotRateSelf;
        }

        if (!lobby.Value.Members.Any(m => m.ProfileId == request.TargetProfileId))
        { 
            return Errors.Profile.NotFound;
        }

        if (!lobby.Value.IsRatingAvailable)
        { 
            return Errors.Ratings.TooEarly;
        }

        bool rateExists = await ratingRepo.AnyByLobbyAndRaterAndTarget(
            request.LobbyId, request.RaterProfileId, request.TargetProfileId, cancellationToken);
        if (rateExists)
        {
            return Errors.Ratings.AlreadyRated;
        }

        var rating = new Rating(request.LobbyId, request.RaterProfileId, request.TargetProfileId, request.Value);
        ratingRepo.Add(rating);

        // 7. Обновить рейтинг цели
        await mediator.Publish(new UserRatedEvent(request.TargetProfileId, request.Value), cancellationToken);

        return Unit.Value;
    }
}
