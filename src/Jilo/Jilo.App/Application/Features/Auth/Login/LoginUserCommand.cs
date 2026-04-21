using Jilo.App.Application.Common.Requests;
using Jilo.App.Domain.Models;

namespace Jilo.App.Application.Features.Auth.Login;

public sealed record LoginUserCommand(string Username, string Password) : ICommand<TokenPair>;
