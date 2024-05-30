using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Iris;
using BlockCiphers;

namespace Iris_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataIrisTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _iris = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptIrisTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _iris = File.ReadAllBytes(IrisGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptIrisTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_iris);
    }

    [GlobalSetup(Target = nameof(DecryptIrisTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedIris = File.ReadAllBytes(IrisGenerator.Generate());
        _iris = _twoFish.Encrypt(generatedIris);
    }

    [Benchmark]
    public byte[] DecryptIrisTwoFish() => _twoFish.Decrypt(_iris);
}