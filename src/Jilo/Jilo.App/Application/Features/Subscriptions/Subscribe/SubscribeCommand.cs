using Jilo.App.Application.Common.Requests;
using Jilo.App.Domain.Models;

namespace Jilo.App.Application.Features.Subscriptions.Subscribe;

public sealed record SubscribeCommand(Guid ProfileId, string RefreshTokenValue) : ICommand<TokenPair>;
