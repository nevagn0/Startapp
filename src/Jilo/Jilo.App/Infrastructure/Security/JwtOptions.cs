namespace Jilo.App.Infrastructure.Security;

public sealed class JwtOptions
{
    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required string Key { get; init; }

    public required int AccessTokenLifetimeSeconds { get; init; }
}
