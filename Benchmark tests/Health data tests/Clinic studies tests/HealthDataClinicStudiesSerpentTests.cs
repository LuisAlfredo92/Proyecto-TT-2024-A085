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
public class HealthDataClinicStudiesSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _clinicStudies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptClinicStudiesSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _clinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicStudiesSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_clinicStudies);
    }

    [GlobalSetup(Target = nameof(DecryptClinicStudiesSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedClinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
        _clinicStudies = _serpent.Encrypt(generatedClinicStudies);
    }

    [Benchmark]
    public byte[] DecryptClinicStudiesSerpent() => _serpent.Decrypt(_clinicStudies);
}