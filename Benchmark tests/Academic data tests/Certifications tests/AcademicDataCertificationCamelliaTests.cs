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
public class AcademicDataCertificationCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _certification = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptCertificationCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _certification = File.ReadAllBytes(CertificationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCertificationCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_certification);
    }

    [GlobalSetup(Target = nameof(DecryptCertificationCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedCertification = File.ReadAllBytes(CertificationsGenerator.Generate());
        _certification = _camellia.Encrypt(generatedCertification);
    }

    [Benchmark]
    public byte[] DecryptCertificationCamellia() => _camellia.Decrypt(_certification);
}