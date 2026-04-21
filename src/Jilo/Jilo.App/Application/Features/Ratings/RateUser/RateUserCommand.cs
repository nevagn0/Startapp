using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Ratings.RateUser;

public sealed record RateUserCommand(Guid LobbyId, Guid RaterProfileId, Guid TargetProfileId, int Value)
    : ICommand;
