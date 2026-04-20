using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using Jilo.App.Applicatoin.DTO;

namespace Jilo.App.Application.Services;

public class PlayerSearchService : IPlayerSearchService
{
    private readonly IPlayerSearchRepository _searchRepository;
    private readonly IGameRepository _gameRepository;

    public PlayerSearchService(
        IPlayerSearchRepository searchRepository,
        IGameRepository gameRepository)
    {
        _searchRepository = searchRepository;
        _gameRepository = gameRepository;
    }

    public async Task<ErrorOr<IEnumerable<PlayerSearchResponse>>> SearchPlayersAsync(
        SearchPlayersRequest request,
        CancellationToken cancellationToken = default)
    {

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game == null)
            return Error.NotFound("Game.NotFound", $"Game with ID '{request.GameId}' not found");

        if (!string.IsNullOrWhiteSpace(request.Rank))
        {
            if (!game.Ranks.Contains(request.Rank))
                return Error.Validation("Rank.Invalid", $"Rank '{request.Rank}' is not available for {game.GameName}");
        }
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!game.Roles.Contains(request.Role))
                return Error.Validation("Role.Invalid", $"Role '{request.Role}' is not available for {game.GameName}");
        }

        var players = await _searchRepository.SearchPlayersAsync(request, cancellationToken);

        return players.ToList();
    }
}