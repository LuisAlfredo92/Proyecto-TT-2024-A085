using FPE_ciphers;

namespace Validation_tests.FPE_ciphers;

public class Cast256FpeTests
{
    private static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [Fact]
    public void TestCase1()
    {
        Span<byte> key = stackalloc byte[32];
        char[] plainData = ['a', 'b', 'c', 'd'];

        Cast256Fpe cast256Fpe = new(key, Alphabet);

        char[] cipherData = cast256Fpe.Encrypt(plainData),
            decryptedData = cast256Fpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        Span<byte> key = stackalloc byte[32];
        var plainData = "Cast256FpeTest".ToArray();

        Cast256Fpe cast256Fpe = new(key, Alphabet);

        char[] cipherData = cast256Fpe.Encrypt(plainData),
            decryptedData = cast256Fpe.Decrypt(cipherData);

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

        Cast256Fpe cast256Fpe = new(key, Alphabet);

        char[] cipherData = cast256Fpe.Encrypt(plainData),
            decryptedData = cast256Fpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}