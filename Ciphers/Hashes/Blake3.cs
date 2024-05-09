using Org.BouncyCastle.Crypto.Digests;

namespace Hashes;

public class Blake3
{
    private static readonly Blake3Digest Blake3Digest = new(1048);

    public static Span<byte> Hash(ReadOnlySpan<byte> input)
    {
        Blake3Digest.BlockUpdate(input);
        var hash = new byte[Blake3Digest.GetDigestSize()];
        Blake3Digest.DoFinal(hash, 0);
        return hash;
    }
}