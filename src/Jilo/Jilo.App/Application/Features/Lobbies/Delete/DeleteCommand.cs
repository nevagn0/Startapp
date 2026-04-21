using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Lobbies.Delete;

public sealed record DeleteCommand(Guid LobbyId) : ICommand;
