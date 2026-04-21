using Jilo.App.Domain.LobbyEntity;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Domain.RatingEntity;

public class Rating
{
    public Guid Id { get; private set; }
    
    public Guid LobbyId { get; private set; }
    
    public Guid RaterProfileId { get; private set; }
    
    public Guid TargetProfileId { get; private set; }
    
    public int Value { get; private set; }
    
    public DateTime CreatedAtUtc { get; private set; }

    public Lobby Lobby { get; private set; } = null!;

    public Profile RaterProfile { get; private set; } = null!;

    public Profile TargetProfile { get; private set; } = null!;

    public Rating(Guid lobbyId, Guid raterProfileId, Guid targetProfileId, int value)
    {
        Id = Guid.NewGuid();
        LobbyId = lobbyId;
        RaterProfileId = raterProfileId;
        TargetProfileId = targetProfileId;
        Value = value;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
