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
public class IdeologicalDataReligionTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _religion = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptReligionTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _religion = ReligionGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptReligionTwoFishFpe() => _twoFishFpe.Encrypt(_religion);

    [GlobalSetup(Target = nameof(DecryptReligionTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedReligion = ReligionGenerator.Generate().ToCharArray();
        _religion = _twoFishFpe.Encrypt(generatedReligion);
    }

    [Benchmark]
    public char[] DecryptReligionTwoFishFpe() => _twoFishFpe.Decrypt(_religion);
}