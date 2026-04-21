using ErrorOr;
using Jilo.App.Applicatoin.Common.Repositories;
using Jilo.App.Applicatoin.Common.Services;
using Jilo.App.Domain.Models;
using Jilo.App.Domain.UserEntity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Jilo.App.Infrastructure.Security;

public sealed class JwtTokenProvider(IOptions<JwtOptions> options, IProfileRepository profileRepo) : ITokenProvider
{
    private readonly JwtOptions _options = options.Value;

    public async Task<ErrorOr<TokenPair>> GetTokensForUser(User user)
    {
        try
        {
            var accessToken = await GetAccessToken(user);
            var refreshToken = GetRefreshToken();

            return new TokenPair(accessToken, refreshToken);
        }
        catch(Exception e)
        {
            return Error.Failure(
                code: "JwtTokenProvider.FailedToCreateTokens",
                description: $"Failed to create token pair. Details {e.Message}");
        }
    }

    private async Task<string> GetAccessToken(User user)
    {
        var profile = await profileRepo.GetByUserIdAsync(user.Id);
        if (profile.IsError)
        {
            throw new Exception("Profile not found");
        }

        var key = Encoding.UTF8.GetBytes(_options.Key);
        var tokenHandler = new JsonWebTokenHandler();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Profile, profile.Value.Id.ToString()),  
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _options.Audience,
            Issuer = _options.Issuer,
            Expires = DateTime.UtcNow.AddSeconds(_options.AccessTokenLifetimeSeconds),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateToken(tokenDescriptor);
    }

    private string GetRefreshToken()
    {
        var bytes = new byte[32];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        
        return Convert.ToBase64String(bytes);
    }
}
