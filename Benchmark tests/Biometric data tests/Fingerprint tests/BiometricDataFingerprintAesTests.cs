using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Fingerprint;
using Aes = BlockCiphers.Aes;

namespace Fingerprint_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataFingerprintAesTests
{
    private Aes _aes = null!;
    private byte[] _fingerprint = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptFingerprintAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _fingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
    }

    [Benchmark]
    public byte[] EncryptFingerprintAes() => _aes.Encrypt(_fingerprint, out _);

    [GlobalSetup(Target = nameof(DecryptFingerprintAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedFingerprint = File.ReadAllBytes(FingerprintGenerator.GenerateFingerprint());
        _fingerprint = _aes.Encrypt(generatedFingerprint, out _tag);
    }

    [Benchmark]
    public byte[] DecryptFingerprintAes() => _aes.Decrypt(_fingerprint, _tag);
}