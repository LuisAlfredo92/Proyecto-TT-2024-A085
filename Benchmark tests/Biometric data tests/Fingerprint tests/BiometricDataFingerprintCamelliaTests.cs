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
public class BiometricDataFingerprintCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _fingerprint = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptFingerprintCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _fingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptFingerprintCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_fingerprint);
    }

    [GlobalSetup(Target = nameof(DecryptFingerprintCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedFingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
        _fingerprint = _camellia.Encrypt(generatedFingerprint);
    }

    [Benchmark]
    public byte[] DecryptFingerprintCamellia() => _camellia.Decrypt(_fingerprint);
}