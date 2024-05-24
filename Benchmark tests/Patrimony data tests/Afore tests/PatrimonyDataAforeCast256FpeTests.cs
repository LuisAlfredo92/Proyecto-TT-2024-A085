using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Afore;

namespace Afore_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _afore = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAforeCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _afore = AforeGenerator.GenerateAforeName().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAforeCast256Fpe() => _cast256Fpe.Encrypt(_afore);

    [GlobalSetup(Target = nameof(DecryptAforeCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedAfore = AforeGenerator.GenerateAforeName().ToCharArray();
        _afore = _cast256Fpe.Encrypt(generatedAfore);
    }

    [Benchmark]
    public char[] DecryptAforeCast256Fpe() => _cast256Fpe.Decrypt(_afore);
}