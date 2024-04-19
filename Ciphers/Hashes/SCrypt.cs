using System.Security.Cryptography;
using static Org.BouncyCastle.Crypto.Generators.SCrypt;

namespace Hashes;

public class SCrypt(byte[]? salt = null, int costParameter = 16384, int blockSize = 8, int parallelizationParameter = 1, int derivedKeyLength = 64)
{
    private readonly byte[] _salt = salt ?? RandomNumberGenerator.GetBytes(16);
    public byte[] Hash(byte[] input)
    {
        return Generate(input, _salt, costParameter, blockSize, parallelizationParameter, derivedKeyLength);
    }
}