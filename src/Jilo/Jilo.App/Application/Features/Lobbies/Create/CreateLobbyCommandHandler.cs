using ErrorOr;
using Jilo.App.Application.Common.Repositories;
using Jilo.App.Domain;
using Jilo.App.Domain.LobbyEntity;
using MediatR;

namespace Jilo.App.Application.Features.Lobbies.Create;

public sealed class CreateLobbyCommandHandler(
    ILobbyRepository lobbyRepo,
    IGameRepository gameRepo)
    : IRequestHandler<CreateLobbyCommand, ErrorOr<CreateLobbyCommandResponse>>
{
    public async Task<ErrorOr<CreateLobbyCommandResponse>> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
    {
        bool inActiveLobby = await lobbyRepo.AnyActiveWithMemberAsync(request.ProfileId, cancellationToken);
        if (inActiveLobby)
        {
            return Errors.Lobby.AlreadyInLobby;
        }

        bool gameExists = await gameRepo.GetByIdAsync(request.GameId, cancellationToken) != null;
        if(!gameExists)
        {
            return Errors.Game.NotFound;
        }

        var lobby = new Lobby(request.GameId, request.ProfileId);

        lobby.AddMember(request.ProfileId);

        lobbyRepo.Add(lobby);

        return new CreateLobbyCommandResponse
        {
            Id = lobby.Id,
            GameId = lobby.GameId,
            CreatedAtUtc = lobby.CreatedAtUtc
        };
    }
}
