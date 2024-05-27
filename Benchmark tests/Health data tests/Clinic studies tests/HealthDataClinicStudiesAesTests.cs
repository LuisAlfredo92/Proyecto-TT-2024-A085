using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Health_data.Clinic_studies;
using Aes = BlockCiphers.Aes;

namespace Clinic_studies_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicStudiesAesTests
{
    private Aes _aes = null!;
    private byte[] _clinicStudies = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptClinicStudiesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _clinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptClinicStudiesAes() => _aes.Encrypt(_clinicStudies, out _);

    [GlobalSetup(Target = nameof(DecryptClinicStudiesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedClinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
        _clinicStudies = _aes.Encrypt(generatedClinicStudies, out _tag);
    }

    [Benchmark]
    public byte[] DecryptClinicStudiesAes() => _aes.Decrypt(_clinicStudies, _tag);
}