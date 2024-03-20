using System.Security.Cryptography;

namespace BlockCiphers;

public class Aes(Span<byte> key, byte[] nonce, byte[]? authenticationData = null)
{
    private readonly byte[] _authenticationData = authenticationData ?? [];
    private readonly AesGcm _algorithm = new(key, AesGcm.TagByteSizes.MaxSize);

    public byte[] Encrypt(byte[] plainText, out byte[] tag)
    {
        var cipherText = new byte[plainText.Length];
        tag = new byte[AesGcm.TagByteSizes.MaxSize];
        _algorithm.Encrypt(nonce, plainText, cipherText, tag, _authenticationData);
        return cipherText;
    }

    public byte[] Decrypt(byte[] cipherText, byte[] tag)
    {
        var plainText = new byte[cipherText.Length];
        _algorithm.Decrypt(nonce, cipherText, tag, plainText, _authenticationData);
        return plainText;
    }
}