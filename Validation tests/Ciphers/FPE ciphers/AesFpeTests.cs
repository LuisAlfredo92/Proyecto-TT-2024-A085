using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FPE_ciphers;
using Xunit.Abstractions;

namespace Validation_tests.Ciphers.FPE_ciphers;

public class AesFpeTests(ITestOutputHelper outputHelper)
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
        Parallel.For(0, plainData.Length, i => plainData[i] = Alphabet[Random.Shared.Next(Alphabet.Length)]);

        AesFpe aesFpe = new(key, Alphabet);

        char[] cipherData = aesFpe.Encrypt(plainData),
            decryptedData = aesFpe.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestTooLongInputs()
    {
        Span<byte> key = stackalloc byte[32];
        var _alphabet = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz .ÁÉÍÓÚáéíóú".ToCharArray();
        Span<char> plainData = stackalloc char[Random.Shared.Next(32, 150)];
        ref var reference = ref MemoryMarshal.GetReference(plainData);
        Random.Shared.NextBytes(key);
        AesFpe aesFpe = new(key, _alphabet);

        // Generate random name
        for (var i = 0; i < plainData.Length; i++)
        {
            Unsafe.Add(ref reference, i) = _alphabet[Random.Shared.Next(_alphabet.Length)];
        }
        outputHelper.WriteLine($"Generated data: {plainData.ToString()}");

        var arrays = Math.Ceiling(plainData.Length / 30f);
        var names = new char[(int)arrays][];
        for (var i = 0; i < names.Length - 1; i++)
        {
            names[i] = plainData.Slice(i * 30, 30).ToArray();
        }
        names[^1] = plainData[((names.Length - 1) * 30)..].ToArray();
        var minLength = Math.Max(names[^1].Length, 4);
        Array.Resize(ref names[^1], minLength);

        for (var i = 0; i < names.Length; i++)
        {
            outputHelper.WriteLine($"Name {i}: {new string(names[i])}");
        }

        var cipherData = new char[(int)arrays][];
        for (var i = 0; i < names.Length; i++)
        {
            cipherData[i] = aesFpe.Encrypt(names[i]);
            outputHelper.WriteLine($"Encrypted name {i}: {new string(cipherData[i])}");
        }

        var decryptedData = new char[(int)arrays][];
        for (var i = 0; i < cipherData.Length; i++)
        {
            decryptedData[i] = aesFpe.Decrypt(cipherData[i]);
            outputHelper.WriteLine($"Decrypted name {i}: {new string(decryptedData[i])}");
        }
    }
}