using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Application.Common.Services;
using Jilo.App.Application.DTO;

namespace Jilo.App.Application.Implementations;

public class PlayerSearchService : IPlayerSearchService
{
    private readonly IPlayerSearchRepository _searchRepository;

    public PlayerSearchService(
        IPlayerSearchRepository searchRepository)
    {
        _searchRepository = searchRepository;
    }

    public async Task<ErrorOr<IEnumerable<PlayerSearchResponse>>> SearchPlayersAsync(
        SearchPlayersRequest request,
        CancellationToken cancellationToken = default)
    {
        var players = await _searchRepository.SearchPlayersAsync(request, cancellationToken);

        return players.ToList();
    }
}