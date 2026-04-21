using ErrorOr;
using Jilo.App.Domain.InvitationEntity;

namespace Jilo.App.Application.Common.Repositories;

public interface IInvitationRepository
{
    void Add(Invitation invitation);

    void Delete(Invitation invitation);
    
    Task<bool> AnyPendingForLobbyForUser(Guid lobbyId, Guid toProfileId, CancellationToken cancellationToken);
    
    Task<ErrorOr<Invitation>> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
