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
public class IdeologicalDataReligionAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _religion = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptReligionAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _religion = ReligionGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptReligionAesFpe() => _aesFpe.Encrypt(_religion);

    [GlobalSetup(Target = nameof(DecryptReligionAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedReligion = ReligionGenerator.Generate().ToCharArray();
        _religion = _aesFpe.Encrypt(generatedReligion);
    }

    [Benchmark]
    public char[] DecryptReligionAesFpe() => _aesFpe.Decrypt(_religion);
}