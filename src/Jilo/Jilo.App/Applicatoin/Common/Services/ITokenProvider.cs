using ErrorOr;
using Jilo.App.Domain;
using Jilo.App.Domain.Models;

namespace Jilo.App.Applicatoin.Common.Services;

public interface ITokenProvider
{
    ErrorOr<TokenPair> GetTokensForUser(User user);
}
