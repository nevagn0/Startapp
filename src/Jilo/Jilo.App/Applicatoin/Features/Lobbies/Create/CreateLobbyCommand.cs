using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Lobbies.Create;

public sealed record CreateLobbyCommand(Guid ProfileId, Guid GameId) : ICommand<CreateLobbyCommandResponse>;
