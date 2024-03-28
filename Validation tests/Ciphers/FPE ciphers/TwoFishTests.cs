using FPE_ciphers;

namespace Validation_tests.Ciphers.FPE_ciphers;

public class TwoFishTests
{
    private static readonly char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [Fact]
    public void TestCase1()
    {
        Span<byte> key = stackalloc byte[32];
        char[] plainData = ['a', 'b', 'c', 'd'];

        TwoFishFpe twoFishFpe = new(key, Alphabet);

        char[] cipherData = twoFishFpe.Encrypt(plainData),
            decryptedData = twoFishFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        Span<byte> key = stackalloc byte[32];
        var plainData = "TwoFishFpeTest".ToArray();

        TwoFishFpe twoFishFpe = new(key, Alphabet);

        char[] cipherData = twoFishFpe.Encrypt(plainData),
            decryptedData = twoFishFpe.Decrypt(cipherData);

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

        TwoFishFpe twoFishFpe = new(key, Alphabet);

        char[] cipherData = twoFishFpe.Encrypt(plainData),
            decryptedData = twoFishFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}