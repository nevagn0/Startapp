using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using MediatR;

namespace Jilo.App.Application.Features.Lobbies.Leave;

public sealed class LeaveLobbyCommandHandler(
    ILobbyRepository repo)
    : IRequestHandler<LeaveLobbyCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(LeaveLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobby = await repo.GetAsync(request.LobbyId, cancellationToken);
        if (lobby.IsError)
        {
            return lobby.Errors;
        }

        if (lobby.Value.CreatedByProfileId == request.ProfileId)
        {
            repo.Delete(lobby.Value);
        }

        var leaveResult = lobby.Value.LeavingBy(request.ProfileId);
        if (leaveResult.IsError)
        {
            return leaveResult.Errors;
        }

        return Unit.Value;
    }
}
