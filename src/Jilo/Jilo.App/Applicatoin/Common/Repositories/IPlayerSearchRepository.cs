namespace Jilo.App.Applicatoin.DTO;
public interface IPlayerSearchRepository
{
    Task<IEnumerable<PlayerSearchResponse>> SearchPlayersAsync(
        SearchPlayersRequest request, CancellationToken cancellationToken = default);
}
