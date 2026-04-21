using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Auth.Logout;

public sealed record LogoutCommand(string RefreshTokenValue) : ICommand;
