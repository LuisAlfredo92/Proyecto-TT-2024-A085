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
public class HealthDataClinicStudiesTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _clinicStudies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptClinicStudiesTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _clinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicStudiesTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_clinicStudies);
    }

    [GlobalSetup(Target = nameof(DecryptClinicStudiesTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedClinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
        _clinicStudies = _twoFish.Encrypt(generatedClinicStudies);
    }

    [Benchmark]
    public byte[] DecryptClinicStudiesTwoFish() => _twoFish.Decrypt(_clinicStudies);
}