using System.Security.Cryptography;

namespace Hashes;

public class Sha2
{
    public static Span<byte> Hash(ReadOnlySpan<byte> input) => SHA512.HashData(input);
}