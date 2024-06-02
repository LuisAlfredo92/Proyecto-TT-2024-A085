using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStateRsaHomomorphicTests
{
    private RsaHomomorphic _rsa = null!;
    private BigInteger _transactionState = null!;
    private RsaKeyPairGenerator? _pGen;
    private AsymmetricCipherKeyPair? _key;

    [GlobalSetup(Target = nameof(EncryptTransactionStateRsaHomomorphic))]
    public void SetupEncryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        _transactionState = BigInteger.ValueOf(Random.Shared.Next());
    }

    [Benchmark]
    public BigInteger EncryptTransactionStateRsaHomomorphic() => _rsa.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateRsaHomomorphic))]
    public void SetupDecryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        var generatedTransactionState = BigInteger.ValueOf(Random.Shared.Next());
        _transactionState = _rsa.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public BigInteger DecryptTransactionStateRsaHomomorphic() => _rsa.Decrypt(_transactionState);
}