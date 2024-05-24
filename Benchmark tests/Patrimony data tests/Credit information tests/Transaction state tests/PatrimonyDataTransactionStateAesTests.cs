using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStateAesTests
{
    private Aes _aes = null!;
    private byte[] _transactionState = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptTransactionStateAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _transactionState = BitConverter.GetBytes(Random.Shared.Next());
    }

    [Benchmark]
    public byte[] EncryptTransactionStateAes() => _aes.Encrypt(_transactionState, out _);

    [GlobalSetup(Target = nameof(DecryptTransactionStateAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedTransactionState = BitConverter.GetBytes(Random.Shared.Next());
        _transactionState = _aes.Encrypt(generatedTransactionState, out _tag);
    }

    [Benchmark]
    public byte[] DecryptTransactionStateAes() => _aes.Decrypt(_transactionState, _tag);
}