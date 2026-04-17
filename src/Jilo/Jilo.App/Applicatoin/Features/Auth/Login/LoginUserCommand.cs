using Jilo.App.Applicatoin.Common.Requests;
using Jilo.App.Domain.Models;

namespace Jilo.App.Applicatoin.Features.Auth.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<TokenPair>;
