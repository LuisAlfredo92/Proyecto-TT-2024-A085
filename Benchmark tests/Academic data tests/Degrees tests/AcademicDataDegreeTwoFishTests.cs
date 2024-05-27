using System.Security.Cryptography;
using Academic_data.Degrees;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Degrees_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class AcademicDataDegreeTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _degree = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptDegreeTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _degree = File.ReadAllBytes(DegreeGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptDegreeTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_degree);
    }

    [GlobalSetup(Target = nameof(DecryptDegreeTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedDegree = File.ReadAllBytes(DegreeGenerator.Generate());
        _degree = _twoFish.Encrypt(generatedDegree);
    }

    [Benchmark]
    public byte[] DecryptDegreeTwoFish() => _twoFish.Decrypt(_degree);
}