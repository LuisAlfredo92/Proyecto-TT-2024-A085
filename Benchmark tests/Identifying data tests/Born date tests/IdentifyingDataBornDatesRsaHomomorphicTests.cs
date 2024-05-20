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
public class IdentifyingDataBornDatesRsaHomomorphicTests
{
    private RsaHomomorphic _rsa = null!;
    private BigInteger _bornDate = null!;
    private RsaKeyPairGenerator? _pGen;
    private AsymmetricCipherKeyPair? _key;

    [GlobalSetup(Target = nameof(EncryptBornDatesRsaHomomorphic))]
    public void SetupEncryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        _bornDate = BigInteger.ValueOf(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public BigInteger EncryptBornDatesRsaHomomorphic() => _rsa.Encrypt(_bornDate);

    [GlobalSetup(Target = nameof(DecryptBornDatesRsaHomomorphic))]
    public void SetupDecryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        var generatedDate = BigInteger.ValueOf(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDate = _rsa.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptBornDatesRsaHomomorphic() => _rsa.Decrypt(_bornDate);
}