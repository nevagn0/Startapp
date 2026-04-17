using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Auth.Logout;

public sealed record LogoutCommand(string RefreshTokenValue) : ICommand;
