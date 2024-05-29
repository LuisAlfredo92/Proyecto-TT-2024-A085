using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Ideological_data.Religion;

namespace Religion_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataReligionTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _religion = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptReligionTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _religion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReligionTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_religion);
    }

    [GlobalSetup(Target = nameof(DecryptReligionTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedReligion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
        _religion = _twoFish.Encrypt(generatedReligion);
    }

    [Benchmark]
    public byte[] DecryptReligionTwoFish() => _twoFish.Decrypt(_religion);
}