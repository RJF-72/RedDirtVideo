using System.Security.Cryptography;
using System.Text;

namespace RedDirtVideo.API.Services;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        var hashBytes = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
        Buffer.BlockCopy(key, 0, hashBytes, salt.Length, key.Length);

        return Convert.ToBase64String(hashBytes);
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        try
        {
            var hashBytes = Convert.FromBase64String(passwordHash);
            var salt = new byte[SaltSize];
            var key = new byte[KeySize];

            Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(hashBytes, salt.Length, key, 0, key.Length);

            var computedKey = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return CryptographicOperations.FixedTimeEquals(computedKey, key);
        }
        catch
        {
            return false;
        }
    }
}
