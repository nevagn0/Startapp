using Jilo.App.Application.Common.Services;
using System.Security.Cryptography;
using System.Text;

namespace Jilo.App.Infrastructure.Security;

public class SHA256TokenHasher : ITokenHasher
{
    public string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);

        var hash = SHA256.HashData(bytes);

        return Convert.ToBase64String(hash);
    }
}
