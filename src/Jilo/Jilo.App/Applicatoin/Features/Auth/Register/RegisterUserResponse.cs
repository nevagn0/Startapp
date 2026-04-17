namespace Jilo.App.Applicatoin.Features.Auth.Register;

public sealed record RegisterUserResponse(Guid Id, string Email, string Username, DateTime CreatedAtUtc, DateTime UpdatedAtUtc);
