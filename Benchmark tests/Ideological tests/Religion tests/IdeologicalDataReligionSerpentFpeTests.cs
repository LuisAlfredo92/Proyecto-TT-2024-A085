using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Ideological_data.Religion;

namespace Religion_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataReligionSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _religion = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptReligionSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _religion = ReligionGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptReligionSerpentFpe() => _serpentFpe.Encrypt(_religion);

    [GlobalSetup(Target = nameof(DecryptReligionSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedReligion = ReligionGenerator.Generate().ToCharArray();
        _religion = _serpentFpe.Encrypt(generatedReligion);
    }

    [Benchmark]
    public char[] DecryptReligionSerpentFpe() => _serpentFpe.Decrypt(_religion);
}