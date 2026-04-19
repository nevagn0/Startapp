using ErrorOr;
using MediatR;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using Jilo.App.Domain.Entities;
using Jilo.App.Applicatoin.Common.Repositories;

namespace Jilo.App.Application.Services;

public class UserGameService : IUserGameService
{
    private readonly IUserGameRepository _userGameRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IProfileRepository _profileRepository;

    public UserGameService(
        IUserGameRepository userGameRepository,
        IGameRepository gameRepository,
        IProfileRepository profileRepository)
    {
        _userGameRepository = userGameRepository;
        _gameRepository = gameRepository;
        _profileRepository = profileRepository;
    }

    public async Task<ErrorOr<Unit>> AddGameToUserAsync(
        Guid userId,
        AddGameToUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game == null)
            return Error.NotFound("Game.NotFound", $"Game with ID '{request.GameId}' not found");

        var profile = await _profileRepository.GetByUserIdAsync(userId, cancellationToken);

        if (profile.IsError)
        {
            return profile.Errors;
        }
        if (string.IsNullOrWhiteSpace(request.Role))
            return Error.Validation("Role.Required", "Role is required");

        if (string.IsNullOrWhiteSpace(request.Rank))
            return Error.Validation("Rank.Required", "Rank is required");

        var userGame = new UserGame(profile.Value.Id, request.GameId, request.Role, request.Rank);
        await _userGameRepository.AddAsync(userGame, cancellationToken);

        return Unit.Value;
    }

    public async Task<ErrorOr<Unit>> UpdateUserGameAsync(
        Guid userId,
        Guid userGameId,
        UpdateUserGameRequest request,
        CancellationToken cancellationToken = default)
    {
        var userGame = await _userGameRepository.GetByIdAsync(userGameId, cancellationToken);
        if (userGame == null)
            return Error.NotFound("UserGame.NotFound", $"User game with ID '{userGameId}' not found");

        var profile = await _profileRepository.GetByUserIdAsync(userId, cancellationToken);

        if (profile.IsError)
        {
            return profile.Errors;
        }

        if (userGame.ProfileId != profile.Value.Id)
            return Error.Unauthorized("UserGame.Unauthorized", "You don't have permission to update this game");

        if (string.IsNullOrWhiteSpace(request.Role))
            return Error.Validation("Role.Required", "Role is required");

        if (string.IsNullOrWhiteSpace(request.Rank))
            return Error.Validation("Rank.Required", "Rank is required");

        userGame.Update(request.Role, request.Rank);
        await _userGameRepository.UpdateAsync(userGame, cancellationToken);

        return Unit.Value;
    }

    public async Task<ErrorOr<Unit>> DeleteUserGameAsync(
        Guid userId,
        Guid userGameId,
        CancellationToken cancellationToken = default)
    {
        var userGame = await _userGameRepository.GetByIdAsync(userGameId, cancellationToken);
        if (userGame == null)
            return Error.NotFound("UserGame.NotFound", $"User game with ID '{userGameId}' not found");

        var profile = await _profileRepository.GetByUserIdAsync(userId, cancellationToken);

        if (profile.IsError)
        {
            return profile.Errors;
        }

        if (userGame.ProfileId != profile.Value.Id)
            return Error.Unauthorized("UserGame.Unauthorized", "You don't have permission to delete this game");

        await _userGameRepository.DeleteAsync(userGameId, cancellationToken);

        return Unit.Value;
    }

    public async Task<ErrorOr<IEnumerable<UserGameResponse>>> GetUserGamesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userGames = await _userGameRepository.GetByUserIdAsync(userId, cancellationToken);

        var response = userGames.Select(ug => new UserGameResponse(
            ug.Id,
            ug.GameId,
            ug.GameCatalog.GameName,
            ug.GameCatalog.Description,
            ug.GameCatalog.CoverImageUrl,
            ug.Role,
            ug.Rank,
            ug.AddedAtUtc
        ));

        return response.ToList();
    }

    public async Task<ErrorOr<IEnumerable<AvailableGameResponse>>> GetAvailableGamesAsync(
        CancellationToken cancellationToken = default)
    {
        var games = await _gameRepository.GetAllAsync(cancellationToken);

        var response = games.Select(g => new AvailableGameResponse(
            g.Id,
            g.GameName,
            g.Description,
            g.CoverImageUrl
        ));

        return response.ToList();
    }

}