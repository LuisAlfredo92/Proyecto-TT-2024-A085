using System.Security.Cryptography;
using Academic_data.Certifications;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Certifications_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataCertificationChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _certification = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptCertificationChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _certification = File.ReadAllBytes(CertificationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCertificationChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_certification);
    }

    [GlobalSetup(Target = nameof(DecryptCertificationChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedCertification = File.ReadAllBytes(CertificationsGenerator.Generate());
        _certification = _chaCha20.Encrypt(generatedCertification);
    }

    [Benchmark]
    public byte[] DecryptCertificationChaCha20() => _chaCha20.Decrypt(_certification);
}