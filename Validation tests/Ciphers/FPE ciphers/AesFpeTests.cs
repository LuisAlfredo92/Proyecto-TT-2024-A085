using FPE_ciphers;

namespace Validation_tests.Ciphers.FPE_ciphers;

public class AesFpeTests
{
    private static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [Fact]
    public void TestCase1()
    {
        Span<byte> key = stackalloc byte[32];
        char[] plainData = ['a', 'b', 'c', 'd'];

        AesFpe aesFpe = new(key, Alphabet);

        char[] cipherData = aesFpe.Encrypt(plainData),
            decryptedData = aesFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        Span<byte> key = stackalloc byte[32];
        var plainData = "AesFpeTest".ToArray();

        AesFpe aesFpe = new(key, Alphabet);

        char[] cipherData = aesFpe.Encrypt(plainData),
            decryptedData = aesFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        Span<byte> key = stackalloc byte[32];
        var plainData = new char[32];
        Random.Shared.NextBytes(key);
        for (var i = 0; i < plainData.Length; i++)
        {
            plainData[i] = Alphabet[Random.Shared.Next(Alphabet.Length)];
        }

        AesFpe aesFpe = new(key, Alphabet);

        char[] cipherData = aesFpe.Encrypt(plainData),
            decryptedData = aesFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}