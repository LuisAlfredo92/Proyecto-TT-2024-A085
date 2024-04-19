using Org.BouncyCastle.Crypto.Digests;

namespace Hashes;

public class Sha3
{
    private readonly Sha3Digest _sha3Digest = new(512);
    public Span<byte> Hash(ReadOnlySpan<byte> input)
    {
        _sha3Digest.BlockUpdate(input);
        var hash = new byte[_sha3Digest.GetDigestSize()];
        _sha3Digest.DoFinal(hash, 0);
        return hash;
    }
}