using Stream_ciphers;

namespace Validation_tests.Stream_ciphers;

public class WakeTests
{
    [Fact]
    public void TestCase1()
    {
        Span<int> key = stackalloc int[4];
        byte[] iv = new byte[8],
            plainData = [];

        Wake wake = new(key);

        byte[] cipherData = wake.Encrypt(plainData),
            decryptedData = wake.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        Span<int> key = stackalloc int[4];
        byte[] iv = new byte[8],
            plainData = "Wake test"u8.ToArray();

        Wake wake = new(key);

        byte[] cipherData = wake.Encrypt(plainData),
            decryptedData = wake.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        Span<int> key = stackalloc int[4];
        byte[] iv = new byte[8],
            associatedData = new byte[50],
            plainData = new byte[2050];
        for(var i = 0; i < 4; i++)
            key[i] = Random.Shared.Next();
        Random.Shared.NextBytes(iv);
        Random.Shared.NextBytes(associatedData);
        Random.Shared.NextBytes(plainData);

        Wake wake = new(key);

        byte[] cipherData = wake.Encrypt(plainData),
            decryptedData = wake.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}