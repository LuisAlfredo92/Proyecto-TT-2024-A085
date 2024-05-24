using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;

namespace Transaction_state_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTransactionStateSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _transactionState = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTransactionStateSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _transactionState = Random.Shared.Next().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptTransactionStateSerpentFpe() => _serpentFpe.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedTransactionState = Random.Shared.Next().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _transactionState = _serpentFpe.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public char[] DecryptTransactionStateSerpentFpe() => _serpentFpe.Decrypt(_transactionState);
}