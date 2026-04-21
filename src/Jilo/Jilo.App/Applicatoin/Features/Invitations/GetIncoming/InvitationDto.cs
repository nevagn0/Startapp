namespace Jilo.App.Applicatoin.Features.Invitations.GetIncoming;

public sealed class InvitationDto
{
    public required Guid Id { get; init; }

    public required Guid GameId { get; init; }

    public required Guid SenderProfileId { get; init; }

    public required Guid LobbyId { get; init; }

    public required string SenderUsername { get; init; }

    public required string GameName { get; init; }

    public required string GameCoverImageUrl { get; init; }

    public required DateTime ExpiresAtUtc { get; init; }

    public required DateTime CreatedAtUtc { get; init; }
}
