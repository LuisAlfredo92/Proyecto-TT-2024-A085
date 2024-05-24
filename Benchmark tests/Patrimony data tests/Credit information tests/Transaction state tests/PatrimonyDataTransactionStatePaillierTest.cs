using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Math;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStatePaillierTests
{
    private Paillier _goldwasserMicali = null!;
    private BigInteger _transactionState = null!;
    private Paillier.PaillierKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptTransactionStatePaillier))]
    public void SetupEncryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        _transactionState = BigInteger.ValueOf(Random.Shared.Next());
    }

    [Benchmark]
    public BigInteger EncryptTransactionStatePaillier() => _goldwasserMicali.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStatePaillier))]
    public void SetupDecryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        var generatedTransactionState = BigInteger.ValueOf(Random.Shared.Next());
        _transactionState = _goldwasserMicali.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public BigInteger DecryptTransactionStatePaillier() => _goldwasserMicali.Decrypt(_transactionState);
}