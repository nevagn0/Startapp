using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Lobbies.Leave;

public sealed record LeaveLobbyCommand(Guid ProfileId, Guid LobbyId) : ICommand;
