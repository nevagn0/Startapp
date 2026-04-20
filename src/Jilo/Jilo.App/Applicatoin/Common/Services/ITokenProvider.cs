using ErrorOr;
using Jilo.App.Domain.Models;
using Jilo.App.Domain.UserEntity;

namespace Jilo.App.Applicatoin.Common.Services;

public interface ITokenProvider
{
    Task<ErrorOr<TokenPair>> GetTokensForUser(User user);
}
