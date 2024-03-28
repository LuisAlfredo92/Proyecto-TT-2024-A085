using BlockCiphers;

namespace Validation_tests.Ciphers.Block_ciphers;

public class BlowfishTests
{
    [Fact]
    public void TestCase1()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = [];

        Blowfish blowfish = new(key, iv);

        byte[] cipherData = blowfish.Encrypt(plainData),
            decryptedData = blowfish.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = "Blowfish test"u8.ToArray();

        Blowfish blowfish = new(key, iv);

        byte[] cipherData = blowfish.Encrypt(plainData),
            decryptedData = blowfish.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = new byte[2050];
        Random.Shared.NextBytes(key);
        Random.Shared.NextBytes(iv);
        Random.Shared.NextBytes(plainData);

        Blowfish blowfish = new(key, iv);

        byte[] cipherData = blowfish.Encrypt(plainData),
            decryptedData = blowfish.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}