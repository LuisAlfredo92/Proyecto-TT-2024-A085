using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Fingerprint;
using BlockCiphers;

namespace Fingerprint_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataFingerprintTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _fingerprint = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptFingerprintTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _fingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptFingerprintTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_fingerprint);
    }

    [GlobalSetup(Target = nameof(DecryptFingerprintTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedFingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
        _fingerprint = _twoFish.Encrypt(generatedFingerprint);
    }

    [Benchmark]
    public byte[] DecryptFingerprintTwoFish() => _twoFish.Decrypt(_fingerprint);
}