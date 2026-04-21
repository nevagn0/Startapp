namespace Jilo.App.Application.Features.Invitations.GetOutgoing;

public sealed class InvitationDto
{
    public required Guid Id { get; init; }

    public required Guid ToProfileId { get; init; }

    public required string RecieverUsername { get; init; }
    
    public required string Status { get; init; }

    public required DateTime CreatedAtUtc { get; init; }
}
