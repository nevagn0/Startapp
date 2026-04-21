using ErrorOr;
using Jilo.App.Application.DTO;
using MediatR;

namespace Jilo.App.Application.Common.Services;

public interface IUserGameService
{
    // Для пользователя
    Task<ErrorOr<Unit>> AddGameToUserAsync(Guid profileId, AddGameToUserRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<Unit>> UpdateUserGameAsync(Guid profileId, Guid userGameId, UpdateUserGameRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<Unit>> DeleteUserGameAsync(Guid profileId, Guid userGameId, CancellationToken cancellationToken = default);

    // Получение игр пользователя
    Task<ErrorOr<IEnumerable<UserGameResponse>>> GetUserGamesAsync(Guid profileId, CancellationToken cancellationToken = default);

    // Игры
    Task<ErrorOr<IEnumerable<AvailableGameResponse>>> GetAvailableGamesAsync(CancellationToken cancellationToken = default);

    Task<ErrorOr<GameDto>> GetGameAsync(Guid gameId, CancellationToken cancellationToken = default);
}