namespace Jilo.App.Api.Dto.Auth;

public sealed record RegisterUserRequest(string Email, string Username, string Password);
