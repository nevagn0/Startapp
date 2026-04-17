using Jilo.App.Applicatoin.Common.Requests;

namespace Jilo.App.Applicatoin.Features.Auth.Register;

public sealed record RegisterUserCommand(string Email, string Username, string Password)
    : ICommand<RegisterUserResponse>;