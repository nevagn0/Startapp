using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Lobbies.Get;

public sealed record GetLobbyQuery(Guid LobbyId) : IQuery<GetLobbyQueryResponse>;
