using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.INE_CIC_numbers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Tests.Identifying_data_tests.INE_CIC_numbers_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicElGamalHomomorphicTests
{
    private ElGamalHomomorphic _elGamal = null!;
    private BigInteger _ineCicNumber = null!;
    private (BigInteger yotta, BigInteger delta) _ineCicNumberEncrypted;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptIneCicElGamalHomomorphic))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _ineCicNumber = BigInteger.ValueOf(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public (BigInteger yotta, BigInteger delta) EncryptIneCicElGamalHomomorphic() => _elGamal.Encrypt(_ineCicNumber);

    [GlobalSetup(Target = nameof(DecryptIneCicElGamalHomomorphic))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedDate = BigInteger.ValueOf(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumberEncrypted = _elGamal.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptIneCicElGamalHomomorphic() => _elGamal.Decrypt(_ineCicNumberEncrypted);
}