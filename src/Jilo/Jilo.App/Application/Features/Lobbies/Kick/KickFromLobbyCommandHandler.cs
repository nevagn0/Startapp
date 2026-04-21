using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using MediatR;

namespace Jilo.App.Application.Features.Lobbies.Kick;

public sealed class KickFromLobbyCommandHandler(
    ILobbyMemberRepository repo)
    : IRequestHandler<KickFromLobbyCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(KickFromLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobbyMember = await repo.FindAsync(request.LobbyId, request.KickedProfileId, cancellationToken);

        if (lobbyMember.IsError)
        {
            return lobbyMember.Errors;
        }

        repo.Delete(lobbyMember.Value);

        return Unit.Value;
    }
}
