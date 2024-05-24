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
public class PatrimonyDataTransactionStateTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _transactionState = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptTransactionStateTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _transactionState = BitConverter.GetBytes(Random.Shared.Next());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTransactionStateTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_transactionState);
    }

    [GlobalSetup(Target = nameof(DecryptTransactionStateTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedTransactionState = BitConverter.GetBytes(Random.Shared.Next());
        _transactionState = _twoFish.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public byte[] DecryptTransactionStateTwoFish() => _twoFish.Decrypt(_transactionState);
}