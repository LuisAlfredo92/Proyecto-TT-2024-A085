using BlockCiphers;

namespace Validation_tests.Ciphers.Block_ciphers;

public class SerpentTests
{
    [Fact]
    public void TestCase1()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = [];

        Serpent serpent = new(key, iv);

        byte[] cipherData = serpent.Encrypt(plainData),
            decryptedData = serpent.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = "Serpent test"u8.ToArray();

        Serpent serpent = new(key, iv);

        byte[] cipherData = serpent.Encrypt(plainData),
            decryptedData = serpent.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            associatedData = new byte[50],
            plainData = new byte[2050];
        Random.Shared.NextBytes(key);
        Random.Shared.NextBytes(iv);
        Random.Shared.NextBytes(associatedData);
        Random.Shared.NextBytes(plainData);

        Serpent serpent = new(key, iv, associatedData);

        byte[] cipherData = serpent.Encrypt(plainData),
            decryptedData = serpent.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}