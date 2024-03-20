using System.Buffers.Binary;
using System.Collections;
using Homomorphic_ciphers;
using Org.BouncyCastle.Math;

namespace Validation_tests.Homomorphic_ciphers;

public class GoldwasserMicaliTests
{
    private static readonly bool[] Values = [true, false, true];

    [Fact]
    public void TestCase1()
    {
        GoldwasserMicali.GmKeyPair key = new(
            new GoldwasserMicali.GmPublicKey(BigInteger.ValueOf(77), BigInteger.ValueOf(6)),
            new GoldwasserMicali.GmPrivateKey(BigInteger.ValueOf(7), BigInteger.ValueOf(11))
        );
        BitArray plainData = new(Values);

        GoldwasserMicali goldwasserMicali = new(key.Public, key.Private);

        var encryptedData = goldwasserMicali.Encrypt(plainData);
        var decryptedData = goldwasserMicali.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        var key = GoldwasserMicali.GenerateKeys();
        BitArray plainData = new(Values);

        GoldwasserMicali goldwasserMicali = new(key.Public, key.Private);

        var encryptedData = goldwasserMicali.Encrypt(plainData);
        var decryptedData = goldwasserMicali.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        var key = GoldwasserMicali.GenerateKeys();
        BitArray a = new(Values),
            b = new(Values),
            ab = new(Values);
        b.Not();
        ab.Xor(b);

        GoldwasserMicali goldwasserMicali = new(key.Public, key.Private);

        BigInteger[] encryptedA = goldwasserMicali.Encrypt(a),
            encryptedB = goldwasserMicali.Encrypt(b),
            encryptedAB = new BigInteger[encryptedA.Length];
        for (var i = 0; i < encryptedAB.Length; i++)
        {
            encryptedAB[i] = encryptedA[i].Multiply(encryptedB[i]).Mod(key.Public.N);
        }
        var decryptedData = goldwasserMicali.Decrypt(encryptedAB);

        Assert.Equal(ab, decryptedData);
    }
    [Fact]
    public void TestCase4()
    {
        var key = GoldwasserMicali.GenerateKeys();
        int aNumber = Random.Shared.Next(int.MaxValue), 
            bNumber = Random.Shared.Next(int.MaxValue);
        Span<byte> aBytes = stackalloc byte[4],
            bBytes = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(aBytes, aNumber);
        BinaryPrimitives.WriteInt32BigEndian(bBytes, bNumber);

        BitArray a = new(aBytes.ToArray()),
            b = new(bBytes.ToArray()),
            ab = new(a);
        ab.Xor(b);

        GoldwasserMicali goldwasserMicali = new(key.Public, key.Private);

        BigInteger[] encryptedA = goldwasserMicali.Encrypt(a),
            encryptedB = goldwasserMicali.Encrypt(b),
            encryptedAB = new BigInteger[encryptedA.Length];
        for (var i = 0; i < encryptedAB.Length; i++)
        {
            encryptedAB[i] = encryptedA[i].Multiply(encryptedB[i]).Mod(key.Public.N);
        }
        var decryptedData = goldwasserMicali.Decrypt(encryptedAB);

        Assert.Equal(ab, decryptedData);
    }
}