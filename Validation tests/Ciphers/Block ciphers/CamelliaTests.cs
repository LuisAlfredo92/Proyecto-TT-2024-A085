using BlockCiphers;

namespace Validation_tests.Ciphers.Block_ciphers;

public class CamelliaTests
{
    [Fact]
    public void TestCase1()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = [];

        Camellia camellia = new(key, iv);

        byte[] cipherData = camellia.Encrypt(plainData),
            decryptedData = camellia.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = "Serpent test"u8.ToArray();

        Camellia camellia = new(key, iv);

        byte[] cipherData = camellia.Encrypt(plainData),
            decryptedData = camellia.Decrypt(cipherData);

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

        Camellia camellia = new(key, iv, associatedData);

        byte[] cipherData = camellia.Encrypt(plainData),
            decryptedData = camellia.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}