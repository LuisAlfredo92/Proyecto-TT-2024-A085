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
public class PatrimonyDataTransactionStateCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _transactionState = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptTransactionStateCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _transactionState = Random.Shared.Next().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptTransactionStateCamelliaFpe() => _camelliaFpe.Encrypt(_transactionState);

    [GlobalSetup(Target = nameof(DecryptTransactionStateCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedTransactionState = Random.Shared.Next().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _transactionState = _camelliaFpe.Encrypt(generatedTransactionState);
    }

    [Benchmark]
    public char[] DecryptTransactionStateCamelliaFpe() => _camelliaFpe.Decrypt(_transactionState);
}