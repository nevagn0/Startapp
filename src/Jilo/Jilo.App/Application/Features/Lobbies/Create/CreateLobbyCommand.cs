using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Lobbies.Create;

public sealed record CreateLobbyCommand(Guid ProfileId, Guid GameId) : ICommand<CreateLobbyCommandResponse>;
