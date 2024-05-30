using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Scars;
using BlockCiphers;

namespace Scar_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataScarTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _scar = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptScarTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _scar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptScarTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_scar);
    }

    [GlobalSetup(Target = nameof(DecryptScarTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedScar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
        _scar = _twoFish.Encrypt(generatedScar);
    }

    [Benchmark]
    public byte[] DecryptScarTwoFish() => _twoFish.Decrypt(_scar);
}