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
public class AcademicDataCertificationTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _certification = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptCertificationTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _certification = File.ReadAllBytes(CertificationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCertificationTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_certification);
    }

    [GlobalSetup(Target = nameof(DecryptCertificationTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedCertification = File.ReadAllBytes(CertificationsGenerator.Generate());
        _certification = _twoFish.Encrypt(generatedCertification);
    }

    [Benchmark]
    public byte[] DecryptCertificationTwoFish() => _twoFish.Decrypt(_certification);
}