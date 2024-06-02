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
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeRsaHomomorphicTests
{
    private RsaHomomorphic _rsa = null!;
    private BigInteger _yourData = null!;
    private RsaKeyPairGenerator? _pGen;
    private AsymmetricCipherKeyPair? _key;

    [GlobalSetup(Target = nameof(EncryptTypeRsaHomomorphic))]
    public void SetupEncryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        _yourData = BigInteger.ValueOf([DataGeneratorNumber]);
    }

    [Benchmark]
    public BigInteger EncryptTypeRsaHomomorphic() => _rsa.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeRsaHomomorphic))]
    public void SetupDecryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        var generatedType = BigInteger.ValueOf([DataGeneratorNumber]);
        _yourData = _rsa.Encrypt(generatedType);
    }

    [Benchmark]
    public BigInteger DecryptTypeRsaHomomorphic() => _rsa.Decrypt(_yourData);
}