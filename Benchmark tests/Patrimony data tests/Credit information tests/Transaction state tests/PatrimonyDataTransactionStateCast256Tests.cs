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
public class PatrimonyDataTransactionStateCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _transactionState = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptTransactionStateCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _transactionState = BitConverter.GetBytes(Random.Shared.Next());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTransactionStateCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_transactionState);
    }

    [GlobalSetup(Target = nameof(DecryptTransactionStateCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedTransactionState = BitConverter.GetBytes(Random.Shared.Next());
        _transactionState = _cast256.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public byte[] DecryptTransactionStateCast256() => _cast256.Decrypt(_transactionState);
}