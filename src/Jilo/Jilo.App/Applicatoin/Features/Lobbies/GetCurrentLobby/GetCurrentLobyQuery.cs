using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Lobbies.GetCurrentLobby;

public sealed record GetCurrentLobyQuery(Guid ProfileId) : IQuery<GetCurrentLobbyQueryResponse>;
