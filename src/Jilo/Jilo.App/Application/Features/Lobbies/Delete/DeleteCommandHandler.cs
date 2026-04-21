using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using MediatR;

namespace Jilo.App.Application.Features.Lobbies.Delete;

public sealed class DeleteCommandHandler(
    ILobbyRepository repo)
    : IRequestHandler<DeleteCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var lobby = await repo.GetAsync(request.LobbyId, cancellationToken);

        if (lobby.IsError)
        {
            return lobby.Errors;
        }

        repo.Delete(lobby.Value);

        return Unit.Value;
    }
}
