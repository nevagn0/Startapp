namespace Jilo.App.Application.Features.Lobbies.GetCurrentLobby;

public sealed class GetCurrentLobbyQueryResponse
{
    public required Guid Id { get; init; }

    public required Guid GameId { get; init; }

    public required DateTime CreatedAtUtc { get; init; }

    public required IReadOnlyList<LobbyMemberDto> Members { get; init; }
}
