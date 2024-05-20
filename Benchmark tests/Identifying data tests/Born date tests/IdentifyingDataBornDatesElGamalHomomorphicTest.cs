using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.Born_dates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Born_date_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesElGamalHomomorphicTests
{
    private ElGamalHomomorphic _elGamal = null!;
    private BigInteger _bornDate = null!;
    private (BigInteger yotta, BigInteger delta) _bornDateEncrypted;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptBornDatesElGamalHomomorphic))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _bornDate = BigInteger.ValueOf(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public (BigInteger yotta, BigInteger delta) EncryptBornDatesElGamalHomomorphic() => _elGamal.Encrypt(_bornDate);

    [GlobalSetup(Target = nameof(DecryptBornDatesElGamalHomomorphic))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedDate = BigInteger.ValueOf(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDateEncrypted = _elGamal.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptBornDatesElGamalHomomorphic() => _elGamal.Decrypt(_bornDateEncrypted);
}