using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.INE_CIC_numbers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Ine_cic_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataIneCicRsaHomomorphicTests
{
    private RsaHomomorphic _rsa = null!;
    private BigInteger _ineCicNumber = null!;
    private RsaKeyPairGenerator? _pGen;
    private AsymmetricCipherKeyPair? _key;

    [GlobalSetup(Target = nameof(EncryptIneCicRsaHomomorphic))]
    public void SetupEncryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        _ineCicNumber = BigInteger.ValueOf(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public BigInteger EncryptIneCicRsaHomomorphic() => _rsa.Encrypt(_ineCicNumber);

    [GlobalSetup(Target = nameof(DecryptIneCicRsaHomomorphic))]
    public void SetupDecryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        var generatedDate = BigInteger.ValueOf(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _rsa.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptIneCicRsaHomomorphic() => _rsa.Decrypt(_ineCicNumber);
}