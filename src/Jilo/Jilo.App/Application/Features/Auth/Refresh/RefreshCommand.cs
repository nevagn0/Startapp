using Jilo.App.Application.Common.Requests;
using Jilo.App.Domain.Models;

namespace Jilo.App.Application.Features.Auth.Refresh;

public sealed record RefreshCommand(string RefreshTokenValue) : ICommand<TokenPair>;
