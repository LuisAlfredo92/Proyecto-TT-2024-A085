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
public class PatrimonyDataAforeAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _afore = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAforeAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _afore = AforeGenerator.GenerateAforeName().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAforeAesFpe() => _aesFpe.Encrypt(_afore);

    [GlobalSetup(Target = nameof(DecryptAforeAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedAfore = AforeGenerator.GenerateAforeName().ToCharArray();
        _afore = _aesFpe.Encrypt(generatedAfore);
    }

    [Benchmark]
    public char[] DecryptAforeAesFpe() => _aesFpe.Decrypt(_afore);
}