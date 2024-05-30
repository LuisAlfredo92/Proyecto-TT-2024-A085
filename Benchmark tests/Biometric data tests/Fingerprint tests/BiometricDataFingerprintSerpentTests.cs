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
public class BiometricDataFingerprintSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _fingerprint = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptFingerprintSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _fingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptFingerprintSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_fingerprint);
    }

    [GlobalSetup(Target = nameof(DecryptFingerprintSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedFingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
        _fingerprint = _serpent.Encrypt(generatedFingerprint);
    }

    [Benchmark]
    public byte[] DecryptFingerprintSerpent() => _serpent.Decrypt(_fingerprint);
}