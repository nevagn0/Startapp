using Jilo.App.Applicatoin.Dto.RegisterUser;

namespace Jilo.App.Applicatoin.Common.Services;

public interface IAuthService
{
    Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
}
