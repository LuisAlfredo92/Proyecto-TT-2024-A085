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
public class PatrimonyDataTransactionStateCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _transactionState = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTransactionStateCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _transactionState = Random.Shared.Next().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptTransactionStateCast256Fpe() => _cast256Fpe.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedTransactionState = Random.Shared.Next().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _transactionState = _cast256Fpe.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public char[] DecryptTransactionStateCast256Fpe() => _cast256Fpe.Decrypt(_transactionState);
}