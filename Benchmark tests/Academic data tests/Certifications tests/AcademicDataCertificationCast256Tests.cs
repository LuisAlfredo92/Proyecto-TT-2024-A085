using System.Security.Cryptography;
using Academic_data.Certifications;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Certifications_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataCertificationCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _certification = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptCertificationCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _certification = File.ReadAllBytes(CertificationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCertificationCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_certification);
    }

    [GlobalSetup(Target = nameof(DecryptCertificationCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedCertification = File.ReadAllBytes(CertificationsGenerator.Generate());
        _certification = _cast256.Encrypt(generatedCertification);
    }

    [Benchmark]
    public byte[] DecryptCertificationCast256() => _cast256.Decrypt(_certification);
}