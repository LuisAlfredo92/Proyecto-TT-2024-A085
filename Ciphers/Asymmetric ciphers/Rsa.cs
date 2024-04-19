using System.Security.Cryptography;

namespace Asymmetric_ciphers;

public class Rsa
{
    private readonly RSA _rsa;

    public Rsa(ReadOnlySpan<byte> key)
    {
        _rsa = RSA.Create();
        _rsa.ImportRSAPrivateKey(key, out _);
    }

    public byte[] Encrypt(ReadOnlySpan<byte> data)
    {
        return _rsa.Encrypt(data.ToArray(), RSAEncryptionPadding.Pkcs1);
    }

    public byte[] Decrypt(ReadOnlySpan<byte> data)
    {
        return _rsa.Decrypt(data.ToArray(), RSAEncryptionPadding.Pkcs1);
    }
}