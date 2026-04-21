using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Lobbies.Delete;

public sealed record DeleteCommand(Guid LobbyId) : ICommand;
