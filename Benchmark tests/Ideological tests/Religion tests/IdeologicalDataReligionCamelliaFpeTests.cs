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
public class IdeologicalDataReligionCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _religion = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptReligionCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _religion = ReligionGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptReligionCamelliaFpe() => _camelliaFpe.Encrypt(_religion);

    [GlobalSetup(Target = nameof(DecryptReligionCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedReligion = ReligionGenerator.Generate().ToCharArray();
        _religion = _camelliaFpe.Encrypt(generatedReligion);
    }

    [Benchmark]
    public char[] DecryptReligionCamelliaFpe() => _camelliaFpe.Decrypt(_religion);
}