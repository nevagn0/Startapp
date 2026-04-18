namespace Jilo.App.Domain.UserEntity;

public sealed class RefreshToken
{
    public Guid Id { get; private init; }

    public Guid UserId { get; private init; }

    public string TokenHash { get; private init; }

    public DateTime CreatedAtUtc { get; private init; }

    public DateTime ExpiresAtUtc { get; private init; }

    public DateTime? RevokedAtUtc { get; private set; }

    public User User { get; private set; } = null!;

    public bool IsActive => ExpiresAtUtc >= DateTime.UtcNow && !RevokedAtUtc.HasValue;

    public RefreshToken(Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAtUtc = DateTime.UtcNow;
        ExpiresAtUtc = expiresAtUtc;
        RevokedAtUtc = null;
    }

    public void Revoke()
    {
        if (RevokedAtUtc.HasValue)
        {
            return;
        }

        RevokedAtUtc = DateTime.UtcNow;
    }
}
