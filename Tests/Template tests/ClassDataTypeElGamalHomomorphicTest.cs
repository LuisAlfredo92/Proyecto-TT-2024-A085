using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class ClassDataTypeElGamalHomomorphicTests
{
    private ElGamalHomomorphic _elGamal = null!;
    private BigInteger _yourData = null!;
    private (BigInteger yotta, BigInteger delta) _yourDataEncrypted;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptTypeElGamalHomomorphic))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _yourData = BigInteger.ValueOf(TypeGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public (BigInteger yotta, BigInteger delta) EncryptTypeElGamalHomomorphic() => _elGamal.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeElGamalHomomorphic))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedDate = BigInteger.ValueOf(TypeGenerator.GenerateBornDate().Ticks);
        _yourDataEncrypted = _elGamal.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptTypeElGamalHomomorphic() => _elGamal.Decrypt(_yourDataEncrypted);
}