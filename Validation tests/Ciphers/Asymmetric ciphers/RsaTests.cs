using System.Security.Cryptography;
using Asymmetric_ciphers;

namespace Validation_tests.Ciphers.Asymmetric_ciphers;

public class RsaTests : IAsyncLifetime
{
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    public Task InitializeAsync()
    {
        return Task.Run(() =>
            {
                _provider = new RSACryptoServiceProvider(16384);
                _key = _provider.ExportRSAPrivateKey();
            }
        );
    }

    public Task DisposeAsync()
    {
        return Task.Run(() => _provider!.Dispose());
    }

    [Fact]
    public void TestCase1()
    {
        byte[] plainData = [];

        Rsa rsa = new(_key);

        byte[] cipherData = rsa.Encrypt(plainData),
            decryptedData = rsa.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        var plainData = "Rsa test"u8.ToArray();

        Rsa rsa = new(_key);

        byte[] cipherData = rsa.Encrypt(plainData),
            decryptedData = rsa.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        var plainData = new byte[2037];
        Random.Shared.NextBytes(plainData);

        Rsa rsa = new(_key);

        byte[] cipherData = rsa.Encrypt(plainData),
            decryptedData = rsa.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}