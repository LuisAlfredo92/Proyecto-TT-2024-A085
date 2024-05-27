using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Health_data.Clinic_studies;

namespace Clinic_studies_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicStudiesCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _clinicStudies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptClinicStudiesCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _clinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicStudiesCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_clinicStudies);
    }

    [GlobalSetup(Target = nameof(DecryptClinicStudiesCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedClinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
        _clinicStudies = _cast256.Encrypt(generatedClinicStudies);
    }

    [Benchmark]
    public byte[] DecryptClinicStudiesCast256() => _cast256.Decrypt(_clinicStudies);
}