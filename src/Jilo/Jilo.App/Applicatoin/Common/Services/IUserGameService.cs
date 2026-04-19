using ErrorOr;
using MediatR;

namespace Jilo.App.Application.Common.Services;

public interface IUserGameService
{
    // Для пользователя
    Task<ErrorOr<Unit>> AddGameToUserAsync(Guid userId, AddGameToUserRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<Unit>> UpdateUserGameAsync(Guid userId, Guid userGameId, UpdateUserGameRequest request, CancellationToken cancellationToken = default);
    Task<ErrorOr<Unit>> DeleteUserGameAsync(Guid userId, Guid userGameId, CancellationToken cancellationToken = default);

    // Получение игр пользователя
    Task<ErrorOr<IEnumerable<UserGameResponse>>> GetUserGamesAsync(Guid userId, CancellationToken cancellationToken = default);


    Task<ErrorOr<IEnumerable<AvailableGameResponse>>> GetAvailableGamesAsync(CancellationToken cancellationToken = default);
}