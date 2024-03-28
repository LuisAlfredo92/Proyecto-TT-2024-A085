using Asymmetric_ciphers;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Validation_tests.Ciphers.Asymmetric_ciphers;

public class ElGamalTests
{
    [Fact]
    public void TestCase1()
    {
        ElGamalParametersGenerator parGen = new();
        parGen.Init(256, 10, new SecureRandom());
        var elParams = parGen.GenerateParameters();
        ElGamalKeyPairGenerator pGen = new();
        pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), elParams));
        var key = pGen.GenerateKeyPair();

        ElGamal elGamal = new((ElGamalPublicKeyParameters)key.Public, (ElGamalPrivateKeyParameters)key.Private);

        byte[] plainData = [],
            cipherData = elGamal.Encrypt(plainData),
            decryptedData = elGamal.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase2()
    {
        ElGamalParametersGenerator parGen = new();
        parGen.Init(256, 10, new SecureRandom());
        var elParams = parGen.GenerateParameters();
        ElGamalKeyPairGenerator pGen = new();
        pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), elParams));
        var key = pGen.GenerateKeyPair();

        ElGamal elGamal = new((ElGamalPublicKeyParameters)key.Public, (ElGamalPrivateKeyParameters)key.Private);

        byte[] plainData = "ElGamal test"u8.ToArray(),
            cipherData = elGamal.Encrypt(plainData),
            decryptedData = elGamal.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }

    [Fact]
    public void TestCase3()
    {
        ElGamalParametersGenerator parGen = new();
        parGen.Init(256, 10, new SecureRandom());
        var elParams = parGen.GenerateParameters();
        ElGamalKeyPairGenerator pGen = new();
        pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), elParams));
        var key = pGen.GenerateKeyPair();

        ElGamal elGamal = new((ElGamalPublicKeyParameters)key.Public, (ElGamalPrivateKeyParameters)key.Private);

        var plainData = new byte[31];
        Random.Shared.NextBytes(plainData);

        byte[] cipherData = elGamal.Encrypt(plainData),
            decryptedData = elGamal.Decrypt(cipherData);

        Assert.Equal(plainData, decryptedData);
    }
}