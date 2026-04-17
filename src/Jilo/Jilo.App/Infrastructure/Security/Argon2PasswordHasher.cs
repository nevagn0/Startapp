using Isopoh.Cryptography.Argon2;
using Jilo.App.Applicatoin.Common.Services;

namespace Jilo.App.Infrastructure.Security;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    private const int TimeCost = 3;          // Количество итераций
    private const int MemoryCost = 65536;    // Используемая память (в KiB), 64 MiB
    private const int Parallelism = 1;       // Степень параллелизма (количество потоков)
    private const Argon2Type Type = Argon2Type.HybridAddressing; // Тип Argon2 (Argon2id)
    private const int HashLength = 32;       // Длина итогового хеша (в байтах)

    public string HashPassword(string password)
    {
        return Argon2.Hash(password, TimeCost, MemoryCost, Parallelism, Type, HashLength);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return Argon2.Verify(passwordHash, password);
    }
}
