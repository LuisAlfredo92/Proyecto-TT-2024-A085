using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Fingerprint;
using Stream_ciphers;

namespace Fingerprint_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataFingerprintChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _fingerprint = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptFingerprintChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _fingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptFingerprintChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_fingerprint);
    }

    [GlobalSetup(Target = nameof(DecryptFingerprintChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedFingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
        _fingerprint = _chaCha20.Encrypt(generatedFingerprint);
    }

    [Benchmark]
    public byte[] DecryptFingerprintChaCha20() => _chaCha20.Decrypt(_fingerprint);
}