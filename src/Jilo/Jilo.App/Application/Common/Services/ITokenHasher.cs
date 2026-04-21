namespace Jilo.App.Application.Common.Services;

public interface ITokenHasher
{
    string HashToken(string token);
}
