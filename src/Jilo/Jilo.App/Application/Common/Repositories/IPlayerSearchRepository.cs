using Jilo.App.Application.DTO;

namespace Jilo.App.Application.Common.Repositories;
public interface IPlayerSearchRepository
{
    Task<IEnumerable<PlayerSearchResponse>> SearchPlayersAsync(
        SearchPlayersRequest request, CancellationToken cancellationToken = default);
}
