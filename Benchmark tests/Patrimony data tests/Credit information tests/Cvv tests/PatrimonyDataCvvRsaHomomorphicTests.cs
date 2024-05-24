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
public class PatrimonyDataCvvRsaHomomorphicTests
{
    private RsaHomomorphic _rsa = null!;
    private BigInteger _cvv = null!;
    private RsaKeyPairGenerator? _pGen;
    private AsymmetricCipherKeyPair? _key;

    [GlobalSetup(Target = nameof(EncryptCvvRsaHomomorphic))]
    public void SetupEncryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        _cvv = BigInteger.ValueOf(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public BigInteger EncryptCvvRsaHomomorphic() => _rsa.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvRsaHomomorphic))]
    public void SetupDecryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        var generatedCvv = BigInteger.ValueOf(CvvGenerator.GenerateCvv());
        _cvv = _rsa.Encrypt(generatedCvv);
    }

    [Benchmark]
    public BigInteger DecryptCvvRsaHomomorphic() => _rsa.Decrypt(_cvv);
}