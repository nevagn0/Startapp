namespace Jilo.App.Applicatoin.Common.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
