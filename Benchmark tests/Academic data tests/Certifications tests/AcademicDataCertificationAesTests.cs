using System.Security.Cryptography;
using Academic_data.Certifications;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Certifications_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataCertificationAesTests
{
    private Aes _aes = null!;
    private byte[] _certification = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptCertificationAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _certification = File.ReadAllBytes(CertificationsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptCertificationAes() => _aes.Encrypt(_certification, out _);

    [GlobalSetup(Target = nameof(DecryptCertificationAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedCertification = File.ReadAllBytes(CertificationsGenerator.Generate());
        _certification = _aes.Encrypt(generatedCertification, out _tag);
    }

    [Benchmark]
    public byte[] DecryptCertificationAes() => _aes.Decrypt(_certification, _tag);
}