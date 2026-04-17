namespace Jilo.App.Applicatoin.Common.Services;

public interface ITokenHasher
{
    string HashToken(string token);
}
