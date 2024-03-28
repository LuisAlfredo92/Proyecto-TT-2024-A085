using System.Security.Cryptography;

namespace Hashes;

public class Pbkdf2(byte[]? salt = null, int iterations = 1000, int outputLength = 32, HashAlgorithmName? hashAlgorithm = null)
{
    private readonly byte[] _salt = salt ?? RandomNumberGenerator.GetBytes(16);
    private readonly HashAlgorithmName _hashAlgorithm = hashAlgorithm ?? HashAlgorithmName.SHA256;
    public byte[] Hash(byte[] input)
    {
        return Rfc2898DeriveBytes.Pbkdf2(input, _salt, iterations, _hashAlgorithm, outputLength);
    }
}