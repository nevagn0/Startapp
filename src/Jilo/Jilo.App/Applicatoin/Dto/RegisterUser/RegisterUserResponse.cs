namespace Jilo.App.Applicatoin.Dto.RegisterUser;

public sealed record RegisterUserResponse(Guid Id, string Email, string Username, DateTime CreatedAtUtc, DateTime UpdatedAtUtc);
