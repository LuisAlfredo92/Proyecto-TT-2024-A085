using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Patrimony_data.Cvv;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvElGamalHomomorphicTests
{
    private ElGamalHomomorphic _elGamal = null!;
    private BigInteger _cvv = null!;
    private (BigInteger yotta, BigInteger delta) _cvvEncrypted;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptCvvElGamalHomomorphic))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _cvv = BigInteger.ValueOf(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public (BigInteger yotta, BigInteger delta) EncryptCvvElGamalHomomorphic() => _elGamal.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvElGamalHomomorphic))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedCvv = BigInteger.ValueOf(CvvGenerator.GenerateCvv());
        _cvvEncrypted = _elGamal.Encrypt(generatedCvv);
    }

    [Benchmark]
    public BigInteger DecryptCvvElGamalHomomorphic() => _elGamal.Decrypt(_cvvEncrypted);
}