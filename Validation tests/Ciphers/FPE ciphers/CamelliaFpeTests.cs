using FPE_ciphers;

namespace Validation_tests.FPE_ciphers;

public class CamelliaFpeTests
{
    private static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [Fact]
    public void TestCase1()
    {
        Span<byte> key = stackalloc byte[32];
        char[] plainData = ['a', 'b', 'c', 'd'];

        CamelliaFpe camelliaFpe = new(key, Alphabet);

        char[] cipherData = camelliaFpe.Encrypt(plainData),
            decryptedData = camelliaFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        Span<byte> key = stackalloc byte[32];
        var plainData = "CamelliaFpeTest".ToArray();

        CamelliaFpe camelliaFpe = new(key, Alphabet);

        char[] cipherData = camelliaFpe.Encrypt(plainData),
            decryptedData = camelliaFpe.Decrypt(cipherData);

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

        CamelliaFpe camelliaFpe = new(key, Alphabet);

        char[] cipherData = camelliaFpe.Encrypt(plainData),
            decryptedData = camelliaFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}