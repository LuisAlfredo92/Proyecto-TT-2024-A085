using Org.BouncyCastle.Crypto.Digests;

namespace Hashes;

public class Blake3
{
    private readonly Blake3Digest _blake3Digest = new(1048);

    public Span<byte> Hash(ReadOnlySpan<byte> input)
    {
        _blake3Digest.BlockUpdate(input);
        var hash = new byte[_blake3Digest.GetDigestSize()];
        _blake3Digest.DoFinal(hash, 0);
        return hash;
    }
}