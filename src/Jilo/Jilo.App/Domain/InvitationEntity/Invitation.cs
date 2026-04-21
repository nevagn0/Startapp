using ErrorOr;
using Jilo.App.Domain.LobbyEntity;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Domain.InvitationEntity;

public sealed class Invitation
{
    public Guid Id { get; private init; }

    public Guid ReceiverProfileId { get; private init; }

    public Guid SenderProfileId { get; private init; }

    public Guid LobbyId { get; private init; }

    public InvitationStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private init; }

    public DateTime ExpiresAtUtc { get; private init; }

    public DateTime? AcceptedAtUtc { get; private set; }

    public DateTime? DeclinedAtUtc { get; private set; }
    
    public DateTime? CanceledAtUtc { get; private set; }

    public Profile ReceiverProfile { get; private init; } = null!;

    public Profile SenderProfile { get; private init; } = null!;

    public Lobby Lobby { get; private init; } = null!;

    public Invitation(Guid receiverProfileId, Guid senderProfileId, Guid lobbyId)
    {
        Id = Guid.NewGuid();
        ReceiverProfileId = receiverProfileId;
        SenderProfileId = senderProfileId;
        LobbyId = lobbyId;
        Status = InvitationStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
        ExpiresAtUtc = DateTime.UtcNow.AddMinutes(5);
    }

    public ErrorOr<Success> Cancel()
    {
        if (Status != InvitationStatus.Pending)
        {
            return Errors.Invitation.CannotCancel;
        }

        Status = InvitationStatus.Canceled;
        CanceledAtUtc = DateTime.UtcNow;
        return Result.Success;
    }

    public ErrorOr<Success> Expired()
    {
        if (Status != InvitationStatus.Pending)
        {
            return Errors.Invitation.AlreadyProcessed;
        }

        Status = InvitationStatus.Expired;

        return Result.Success;
    }

    public ErrorOr<Success> Accept()
    {
        if (Status != InvitationStatus.Pending)
        {
            return Errors.Invitation.AlreadyProcessed;
        }

        Status = InvitationStatus.Accepted;
        AcceptedAtUtc = DateTime.UtcNow;

        return Result.Success;
    }

    public ErrorOr<Success> Decline()
    {
        if (Status != InvitationStatus.Pending)
        {
            return Errors.Invitation.AlreadyProcessed;
        }

        Status = InvitationStatus.Declined;
        DeclinedAtUtc = DateTime.UtcNow;

        return Result.Success;
    }
}
