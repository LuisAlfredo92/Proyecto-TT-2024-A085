using System.Text;
using BlockCiphers;
using Identifying_data.Names;

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

    [Fact]
    public void TestCase4()
    {
        Span<byte> key = stackalloc byte[32],
        iv = stackalloc byte[8];

        for (var i = 0; i < 1000; i++)
        {
            Random.Shared.NextBytes(key);
            Random.Shared.NextBytes(iv);
            var plainData = Encoding.UTF8.GetBytes(NamesGenerator.Generate());

            Blowfish blowfish = new(key.ToArray(), iv.ToArray());

            byte[] cipherData = blowfish.Encrypt(plainData),
                decryptedData = blowfish.Decrypt(cipherData);

            Assert.Equal(plainData, decryptedData);
        }
    }
}