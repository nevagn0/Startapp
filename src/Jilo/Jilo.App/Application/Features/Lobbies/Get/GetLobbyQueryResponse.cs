namespace Jilo.App.Application.Features.Lobbies.Get;

public sealed class GetLobbyQueryResponse
{
    public required Guid Id { get; init; }

    public required Guid GameId { get; init; }

    public required DateTime CreatedAtUtc { get; init; }

    public required bool IsActive { get; init; }

    public required bool IsRatingAvailable { get; init; }

    public required IReadOnlyList<LobbyMemberDto> Members { get; init; }
}
