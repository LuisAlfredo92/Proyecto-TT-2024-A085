using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStateCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _transactionState = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptTransactionStateCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _transactionState = BitConverter.GetBytes(Random.Shared.Next());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTransactionStateCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_transactionState);
    }

    [GlobalSetup(Target = nameof(DecryptTransactionStateCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedTransactionState = BitConverter.GetBytes(Random.Shared.Next());
        _transactionState = _camellia.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public byte[] DecryptTransactionStateCamellia() => _camellia.Decrypt(_transactionState);
}