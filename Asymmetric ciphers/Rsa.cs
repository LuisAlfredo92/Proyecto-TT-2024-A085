using System.Security.Cryptography;

namespace Asymmetric_ciphers;

public class Rsa
{
    private readonly RSA rsa;

    public Rsa(ReadOnlySpan<byte> key)
    {
        rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(key, out _);
    }

    public byte[] Encrypt(ReadOnlySpan<byte> data)
    {
        return rsa.Encrypt(data.ToArray(), RSAEncryptionPadding.Pkcs1);
    }

    public byte[] Decrypt(ReadOnlySpan<byte> data)
    {
        return rsa.Decrypt(data.ToArray(), RSAEncryptionPadding.Pkcs1);
    }
}