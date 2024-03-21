using System.Security.Cryptography;

namespace Hashes
{
    public class Sha2
    {
        public Span<byte> Hash(ReadOnlySpan<byte> input)
        {
            return SHA512.HashData(input);
        } 
    }
}
