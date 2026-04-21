using ErrorOr;
using Jilo.App.Application.DTO;

namespace Jilo.App.Application.Common.Services;

public interface IPlayerSearchService
{
    Task<ErrorOr<IEnumerable<PlayerSearchResponse>>> SearchPlayersAsync(
        SearchPlayersRequest request,
        CancellationToken cancellationToken = default);
}