using Homomorphic_ciphers;
using Org.BouncyCastle.Math;

namespace Validation_tests.Homomorphic_ciphers;

public class BenalohTests
{
    [Fact]
    public void TestCase1()
    {
        Benaloh.BenalohKeyPair key = new(
            new Benaloh.BenalohPublicKey(BigInteger.ValueOf(13213), BigInteger.ValueOf(99), BigInteger.ValueOf(75827)),
            new Benaloh.BenalohPrivateKey(BigInteger.ValueOf(397), BigInteger.ValueOf(191))
        );
        BigInteger plainData = BigInteger.ValueOf(78),
            expectedData = BigInteger.ValueOf(67158);

        Benaloh benaloh = new(key.Public, key.Private);

        BigInteger encryptedData = benaloh.Encrypt(plainData),
            decryptedData = benaloh.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
        // To test encrypted data, u must be equal to 66183
        //Assert.Equal(expectedData, encryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        //var key = Benaloh.GenerateKeys();
        Benaloh.BenalohKeyPair key = new(
            new Benaloh.BenalohPublicKey(BigInteger.ValueOf(13213), BigInteger.ValueOf(99), BigInteger.ValueOf(75827)),
            new Benaloh.BenalohPrivateKey(BigInteger.ValueOf(397), BigInteger.ValueOf(191))
        );
        var plainData = BigInteger.ValueOf(128);

        Benaloh benaloh = new(key.Public, key.Private);

        BigInteger encryptedData = benaloh.Encrypt(plainData),
            decryptedData = benaloh.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        //var key = Benaloh.GenerateKeys();
        Benaloh.BenalohKeyPair key = new(
            new Benaloh.BenalohPublicKey(BigInteger.ValueOf(13213), BigInteger.ValueOf(99), BigInteger.ValueOf(75827)),
            new Benaloh.BenalohPrivateKey(BigInteger.ValueOf(397), BigInteger.ValueOf(191))
        );
        var plainData = BigInteger.ValueOf(Random.Shared.Next(key.Public.R.IntValue));

        Benaloh benaloh = new(key.Public, key.Private);

        BigInteger encryptedData = benaloh.Encrypt(plainData),
            decryptedData = benaloh.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase4()
    {
        //var key = Benaloh.GenerateKeys();
        Benaloh.BenalohKeyPair key = new(
            new Benaloh.BenalohPublicKey(BigInteger.ValueOf(13213), BigInteger.ValueOf(99), BigInteger.ValueOf(75827)),
            new Benaloh.BenalohPrivateKey(BigInteger.ValueOf(397), BigInteger.ValueOf(191))
        );
        var plainData = BigInteger.ValueOf(Random.Shared.Next(key.Public.R.IntValue));

        Benaloh benaloh = new(key.Public, key.Private);

        BigInteger encryptedData = benaloh.Encrypt(plainData),
            decryptedData = benaloh.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }
}