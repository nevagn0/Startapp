using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Lobbies.Kick;

public sealed record KickFromLobbyCommand(Guid LobbyId, Guid KickedProfileId) : ICommand;
