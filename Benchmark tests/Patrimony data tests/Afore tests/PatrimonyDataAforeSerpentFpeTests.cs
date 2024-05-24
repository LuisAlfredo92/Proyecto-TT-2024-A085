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
public class PatrimonyDataAforeSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _afore = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptAforeSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _afore = AforeGenerator.GenerateAforeName().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptAforeSerpentFpe() => _serpentFpe.Encrypt(_afore);

    [GlobalSetup(Target = nameof(DecryptAforeSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedAfore = AforeGenerator.GenerateAforeName().ToCharArray();
        _afore = _serpentFpe.Encrypt(generatedAfore);
    }

    [Benchmark]
    public char[] DecryptAforeSerpentFpe() => _serpentFpe.Decrypt(_afore);
}