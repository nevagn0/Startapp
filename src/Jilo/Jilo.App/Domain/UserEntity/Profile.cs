namespace Jilo.App.Domain.UserEntity;

public sealed class Profile
{
    private int _rating;

    public Guid Id { get; private init; }

    public Guid UserId { get; private init; }

    public string Username { get; private set; }

    public string Bio { get; private set; }

    public string? AvatarUrl { get; private set; }

    public User User { get; private init; } = null!;

    public int Rating => _rating;

    public Profile(Guid userId, string username, string bio, string? avatarUrl = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Username = username;
        Bio = bio;
        _rating = 0;
        AvatarUrl = avatarUrl;
    }

    public void IncreaseRating()
    {
        Interlocked.Increment(ref _rating);
    }

    public void DecreaseRating()
    {
        Interlocked.Decrement(ref _rating);
    }
}
