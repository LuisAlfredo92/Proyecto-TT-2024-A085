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
public class IdeologicalDataReligionCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _religion = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptReligionCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _religion = ReligionGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptReligionCast256Fpe() => _cast256Fpe.Encrypt(_religion);

    [GlobalSetup(Target = nameof(DecryptReligionCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedReligion = ReligionGenerator.Generate().ToCharArray();
        _religion = _cast256Fpe.Encrypt(generatedReligion);
    }

    [Benchmark]
    public char[] DecryptReligionCast256Fpe() => _cast256Fpe.Decrypt(_religion);
}