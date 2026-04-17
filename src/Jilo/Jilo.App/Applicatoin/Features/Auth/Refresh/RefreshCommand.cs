using Jilo.App.Applicatoin.Common.Requests;
using Jilo.App.Domain.Models;

namespace Jilo.App.Applicatoin.Features.Auth.Refresh;

public sealed record RefreshCommand(string RefreshTokenValue) : ICommand<TokenPair>;
