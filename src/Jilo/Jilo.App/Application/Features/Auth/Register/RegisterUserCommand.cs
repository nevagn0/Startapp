using Jilo.App.Application.Common.Requests;

namespace Jilo.App.Application.Features.Auth.Register;

public sealed record RegisterUserCommand(string Email, string Username, string Password)
    : ICommand<RegisterUserResponse>;