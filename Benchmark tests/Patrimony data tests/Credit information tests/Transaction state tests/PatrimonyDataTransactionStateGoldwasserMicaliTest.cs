using System.Collections;
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
public class PatrimonyDataTransactionStateGoldwasserMicaliTests
{
    private GoldwasserMicali _goldwasserMicali = null!;
    private byte[] _transactionState = null!;
    private BigInteger[]? _transactionStateEncrypted;
    private GoldwasserMicali.GmKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptTransactionStateGoldwasserMicali))]
    public void SetupEncryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        _transactionState = BitConverter.GetBytes(Random.Shared.Next());
    }

    [Benchmark]
    public BigInteger[] EncryptTransactionStateGoldwasserMicali() => _goldwasserMicali.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateGoldwasserMicali))]
    public void SetupDecryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        var generatedTransactionState = BitConverter.GetBytes(Random.Shared.Next());
        _transactionStateEncrypted = _goldwasserMicali.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public BitArray DecryptTransactionStateGoldwasserMicali() => _goldwasserMicali.Decrypt(_transactionStateEncrypted!);
}