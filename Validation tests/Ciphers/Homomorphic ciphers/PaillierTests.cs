using Homomorphic_ciphers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Validation_tests.Ciphers.Homomorphic_ciphers;

public class PaillierTests
{
    [Fact]
    public void TestCase1()
    {
        var key = Paillier.GenerateKeys();
        var plainData = BigInteger.ValueOf(5);

        Paillier paillier = new(key.Public, key.Private);

        BigInteger encryptedData = paillier.Encrypt(plainData),
            decryptedData = paillier.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        var key = Paillier.GenerateKeys();
        var plainData = BigIntegers.CreateRandomInRange(BigInteger.Zero, key.Public.N, new SecureRandom());

        Paillier paillier = new(key.Public, key.Private);

        BigInteger encryptedData = paillier.Encrypt(plainData),
            decryptedData = paillier.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    /// <summary>
    /// Test the homomorphic addition property of the Paillier cryptosystem
    /// </summary>
    [Fact]
    public void TestCase3()
    {
        var key = Paillier.GenerateKeys();
        var secureRandom = new SecureRandom();
        BigInteger a = BigIntegers.CreateRandomInRange(BigInteger.Zero, key.Public.N, secureRandom),
            b = BigIntegers.CreateRandomInRange(BigInteger.Zero, key.Public.N, secureRandom),
            aPlusB = a.Add(b).Mod(key.Public.N);

        Paillier paillier = new(key.Public, key.Private);

        BigInteger encryptedA = paillier.Encrypt(a),
            encryptedB = paillier.Encrypt(b),
            encryptedAB = encryptedA.ModMultiply(encryptedB, key.Public.N.Pow(2)),
            decryptedAB = paillier.Decrypt(encryptedAB);

        Assert.Equal(aPlusB, decryptedAB);
    }

    /// <summary>
    /// Test the homomorphic multiplication property of the Paillier cryptosystem
    /// </summary>
    [Fact]
    public void TestCase4()
    {
        var key = Paillier.GenerateKeys();
        var secureRandom = new SecureRandom();
        BigInteger a = BigIntegers.CreateRandomInRange(BigInteger.Zero, key.Public.N, secureRandom),
            b = BigIntegers.CreateRandomInRange(BigInteger.Zero, key.Public.N, secureRandom),
            aTimesB = a.ModMultiply(b, key.Public.N);

        Paillier paillier = new(key.Public, key.Private);

        BigInteger encryptedA = paillier.Encrypt(a),
            encryptedAPowB = encryptedA.ModPow(b, key.Public.N.Pow(2)),
            decryptedAB = paillier.Decrypt(encryptedAPowB);

        Assert.Equal(aTimesB, decryptedAB);
    }
}