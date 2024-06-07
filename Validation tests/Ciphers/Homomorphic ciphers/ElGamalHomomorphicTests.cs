using Homomorphic_ciphers;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Validation_tests.Ciphers.Homomorphic_ciphers;

public class ElGamalHomomorphicTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void TestCase1()
    {
        ElGamalParametersGenerator parGen = new();
        var secureRandom = new SecureRandom();
        parGen.Init(256, 10, secureRandom);
        var elParams = parGen.GenerateParameters();
        ElGamalKeyPairGenerator pGen = new();
        pGen.Init(new ElGamalKeyGenerationParameters(secureRandom, elParams));
        var key = pGen.GenerateKeyPair();

        BigInteger plainData = new("123456789");

        ElGamalHomomorphic elGamalHomomorphic = new((key.Public as ElGamalPublicKeyParameters)!, (key.Private as ElGamalPrivateKeyParameters)!);

        // Encrypt and decrypt test
        (BigInteger, BigInteger) encryptedData = elGamalHomomorphic.Encrypt(plainData);
        var decryptedData = elGamalHomomorphic.Decrypt(encryptedData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        ElGamalParametersGenerator parGen = new();
        var secureRandom = new SecureRandom();
        parGen.Init(256, 10, secureRandom);
        var elParams = parGen.GenerateParameters();
        ElGamalKeyPairGenerator pGen = new();
        pGen.Init(new ElGamalKeyGenerationParameters(secureRandom, elParams));
        var key = pGen.GenerateKeyPair();

        BigInteger a = new("100"),
            b = BigInteger.Two,
            ab = a.Multiply(b);
        testOutputHelper.WriteLine($"{a} * {b} = {ab}");

        ElGamalHomomorphic elGamalHomomorphic = new((key.Public as ElGamalPublicKeyParameters)!, (key.Private as ElGamalPrivateKeyParameters)!);
        
        // Homomorphic property test
        (BigInteger, BigInteger) encryptedA = elGamalHomomorphic.Encrypt(a),
            encryptedB = elGamalHomomorphic.Encrypt(b),
            encryptedAEncryptedB = (encryptedA.Item1.Multiply(encryptedB.Item1), encryptedA.Item2.Multiply(encryptedB.Item2).Mod(elParams.P));
        var decryptedADecryptedB = elGamalHomomorphic.Decrypt(encryptedAEncryptedB);
        testOutputHelper.WriteLine($"{decryptedADecryptedB}");

        Assert.Equal(ab, decryptedADecryptedB);
    }
    [Fact]
    public void TestCase3()
    {
        ElGamalParametersGenerator parGen = new();
        var secureRandom = new SecureRandom();
        parGen.Init(256, 10, secureRandom);
        var elParams = parGen.GenerateParameters();
        ElGamalKeyPairGenerator pGen = new();
        pGen.Init(new ElGamalKeyGenerationParameters(secureRandom, elParams));
        var key = pGen.GenerateKeyPair();

        BigInteger a = new("100"),
            b = BigInteger.Zero,
            ab = a.Multiply(b);
        testOutputHelper.WriteLine($"{a} * {b} = {ab}");

        ElGamalHomomorphic elGamalHomomorphic = new((key.Public as ElGamalPublicKeyParameters)!, (key.Private as ElGamalPrivateKeyParameters)!);
        
        // Homomorphic property test
        (BigInteger, BigInteger) encryptedA = elGamalHomomorphic.Encrypt(a),
            encryptedB = elGamalHomomorphic.Encrypt(b),
            encryptedAEncryptedB = (encryptedA.Item1.Multiply(encryptedB.Item1), encryptedA.Item2.Multiply(encryptedB.Item2).Mod(elParams.P));
        var decryptedADecryptedB = elGamalHomomorphic.Decrypt(encryptedAEncryptedB);
        testOutputHelper.WriteLine($"{decryptedADecryptedB}");

        Assert.Equal(ab, decryptedADecryptedB);
    }
}