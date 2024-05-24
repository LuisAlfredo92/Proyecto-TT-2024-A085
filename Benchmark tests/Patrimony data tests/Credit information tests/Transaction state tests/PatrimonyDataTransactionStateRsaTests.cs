using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStateRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _transactionState = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptTransactionStateRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _transactionState = BitConverter.GetBytes(Random.Shared.Next());
    }

    [Benchmark]
    public byte[] EncryptTransactionStateRsa() => _rsa.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedTransactionState = BitConverter.GetBytes(Random.Shared.Next());
        _transactionState = _rsa.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public byte[] DecryptTransactionStateRsa() => _rsa.Decrypt(_transactionState);
}