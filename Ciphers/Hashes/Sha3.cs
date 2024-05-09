using Org.BouncyCastle.Crypto.Digests;

namespace Hashes;

public class Sha3
{
    private static readonly Sha3Digest Sha3Digest = new(512);
    public static Span<byte> Hash(ReadOnlySpan<byte> input)
    {
        Sha3Digest.BlockUpdate(input);
        var hash = new byte[Sha3Digest.GetDigestSize()];
        Sha3Digest.DoFinal(hash, 0);
        return hash;
    }
}