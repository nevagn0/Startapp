using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Lobbies.Kick;

public sealed record KickFromLobbyCommand(Guid LobbyId, Guid KickedProfileId) : ICommand;
