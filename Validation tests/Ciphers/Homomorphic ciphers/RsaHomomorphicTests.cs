using Homomorphic_ciphers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Validation_tests.Ciphers.Homomorphic_ciphers;

public class RsaHomomorphicTests
{
    [Fact]
    public void TestCase1()
    {
        RsaKeyPairGenerator pGen = new();
        pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        var key = pGen.GenerateKeyPair();
        BigInteger plainData = new("123456789");

        RsaHomomorphic rsaHomomorphic = new((key.Public as RsaKeyParameters)!, (key.Private as RsaKeyParameters)!);

        // Encrypt and decrypt test
        BigInteger encryptedData = rsaHomomorphic.Encrypt(plainData),
            decryptedData = rsaHomomorphic.Decrypt(encryptedData);
        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        RsaKeyPairGenerator pGen = new();
        pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        var key = pGen.GenerateKeyPair();
        BigInteger a = new("100"),
            b = BigInteger.Two,
            ab = a.Multiply(b);

        RsaHomomorphic rsaHomomorphic = new((key.Public as RsaKeyParameters)!, (key.Private as RsaKeyParameters)!);

        // Encrypt and decrypt test
        BigInteger encryptedA = rsaHomomorphic.Encrypt(a),
            encryptedB = rsaHomomorphic.Encrypt(b),
            encryptedAEncryptedB = encryptedA.Multiply(encryptedB),
            decryptedAdecryptedB = rsaHomomorphic.Decrypt(encryptedAEncryptedB);

        Assert.Equal(ab, decryptedAdecryptedB);
    }
}