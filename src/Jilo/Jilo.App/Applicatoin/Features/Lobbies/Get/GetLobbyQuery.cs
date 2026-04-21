using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Lobbies.Get;

public sealed record GetLobbyQuery(Guid LobbyId) : IQuery<GetLobbyQueryResponse>;
