using BlockCiphers;

namespace Validation_tests.Block_ciphers;

public class Cast256Tests
{
    [Fact]
    public void TestCase1()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = [];

        Cast256 cast256 = new(key, iv);

        byte[] cipherData = cast256.Encrypt(plainData),
            decryptedData = cast256.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        byte[] key = new byte[32],
            iv = new byte[8],
            plainData = "Cast256 test"u8.ToArray();

        Cast256 cast256 = new(key, iv);

        byte[] cipherData = cast256.Encrypt(plainData),
            decryptedData = cast256.Decrypt(cipherData);

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

        Cast256 cast256 = new(key, iv, associatedData);

        byte[] cipherData = cast256.Encrypt(plainData),
            decryptedData = cast256.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}