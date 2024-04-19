using System.Security.Cryptography;

namespace Hashes;

public class BCrypt(byte[]? salt = null, int cost = 4)
{
    private readonly byte[] _salt = salt ?? RandomNumberGenerator.GetBytes(16);
    public byte[] Hash(byte[] input)
    {
        return Org.BouncyCastle.Crypto.Generators.BCrypt.Generate(input, _salt, cost);
    }
}