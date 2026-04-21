using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Lobbies.Leave;

public sealed record LeaveLobbyCommand(Guid ProfileId, Guid LobbyId) : ICommand;
