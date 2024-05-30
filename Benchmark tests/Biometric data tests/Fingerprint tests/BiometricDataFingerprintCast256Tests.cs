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
public class BiometricDataFingerprintCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _fingerprint = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptFingerprintCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _fingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptFingerprintCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_fingerprint);
    }

    [GlobalSetup(Target = nameof(DecryptFingerprintCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedFingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
        _fingerprint = _cast256.Encrypt(generatedFingerprint);
    }

    [Benchmark]
    public byte[] DecryptFingerprintCast256() => _cast256.Decrypt(_fingerprint);
}