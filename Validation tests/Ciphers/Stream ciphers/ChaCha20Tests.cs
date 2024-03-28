using Stream_ciphers;

namespace Validation_tests.Stream_ciphers;

public class ChaCha20Tests
{
    /// <summary>
    /// Tests the default ChaCha20 encryption and decryption
    /// </summary>
    [Fact]
    public void TestCase1()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] nonce = new byte[12],
            plainBytes = [];
        Random.Shared.NextBytes(plainBytes);
        Random.Shared.NextBytes(nonce);
        Random.Shared.NextBytes(key);

        ChaCha20 chaCha20 = new(key, nonce);

        var encrypted = chaCha20.Encrypt(plainBytes);
        var decrypted = chaCha20.Decrypt(encrypted);

        Assert.Equal(plainBytes, decrypted);
    }

    [Fact]
    public void TestCase2()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] nonce = new byte[12],
            plainData = "ChaCha20 test"u8.ToArray();

        ChaCha20 chaCha20 = new(key, nonce);

        byte[] cipherData = chaCha20.Encrypt(plainData),
            decryptedData = chaCha20.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] nonce = new byte[12],
            associatedData = new byte[50],
            plainData = new byte[2050];
        Random.Shared.NextBytes(key);
        Random.Shared.NextBytes(nonce);
        Random.Shared.NextBytes(associatedData);
        Random.Shared.NextBytes(plainData);

        ChaCha20 chaCha20 = new(key, nonce, associatedData);

        byte[] cipherData = chaCha20.Encrypt(plainData),
            decryptedData = chaCha20.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}