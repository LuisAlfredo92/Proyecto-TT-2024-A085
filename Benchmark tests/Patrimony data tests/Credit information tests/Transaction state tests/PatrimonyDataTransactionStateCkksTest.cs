using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Microsoft.Research.SEAL;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStateCkksTests
{
    private Ckks _ckks = null!;
    private long _transactionState;
    private Ciphertext? _transactionStateEncrypted;

    [GlobalSetup(Target = nameof(EncryptTransactionStateCkks))]
    public void SetupEncryption()
    {
        _ckks = new Ckks();
        _transactionState = Random.Shared.Next();
    }

    [Benchmark]
    public Ciphertext EncryptTransactionStateCkks() => _ckks.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateCkks))]
    public void SetupDecryption()
    {
        _ckks = new Ckks();

        var generatedTransactionState = Random.Shared.Next();
        _transactionStateEncrypted = _ckks.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public long DecryptTransactionStateCkks() => _ckks.Decrypt(_transactionStateEncrypted!);
}