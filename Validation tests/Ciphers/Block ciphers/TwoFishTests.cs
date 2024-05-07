using BlockCiphers;

namespace Validation_tests.Ciphers.Block_ciphers;

public class TwoFishTests
{
    [Fact]
    public void TestCase1()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] iv = new byte[8],
            plainData = [];

        TwoFish twoFish = new(key, iv);

        byte[] cipherData = twoFish.Encrypt(plainData),
            decryptedData = twoFish.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] iv = new byte[8],
            plainData = "TwoFish test"u8.ToArray();

        TwoFish twoFish = new(key, iv);

        byte[] cipherData = twoFish.Encrypt(plainData),
            decryptedData = twoFish.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] iv = new byte[8],
            associatedData = new byte[50],
            plainData = new byte[2050];
        Random.Shared.NextBytes(key);
        Random.Shared.NextBytes(iv);
        Random.Shared.NextBytes(associatedData);
        Random.Shared.NextBytes(plainData);

        TwoFish twoFish = new(key, iv, associatedData);

        byte[] cipherData = twoFish.Encrypt(plainData),
            decryptedData = twoFish.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase4()
    {
        Span<byte> key = stackalloc byte[32];
        byte[] iv = new byte[8],
            associatedData = new byte[50],
            plainData = new byte[2050];
        Random.Shared.NextBytes(key);
        Random.Shared.NextBytes(iv);
        Random.Shared.NextBytes(associatedData);
        Random.Shared.NextBytes(plainData);

        TwoFish twoFish = new(key, iv, associatedData);

        byte[] cipherData = twoFish.Encrypt(plainData),
            decryptedData = twoFish.Decrypt(cipherData);
        twoFish.Reset();
        byte[] cipherData2 = twoFish.Encrypt(plainData),
            decryptedData2 = twoFish.Decrypt(cipherData2);

        Assert.Equal(plainData, decryptedData);
        Assert.Equal(plainData, decryptedData2);
    }
}