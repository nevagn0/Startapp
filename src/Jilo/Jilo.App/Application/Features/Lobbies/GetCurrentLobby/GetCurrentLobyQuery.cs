using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Lobbies.GetCurrentLobby;

public sealed record GetCurrentLobyQuery(Guid ProfileId) : IQuery<GetCurrentLobbyQueryResponse>;
