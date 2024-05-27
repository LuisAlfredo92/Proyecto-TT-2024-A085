using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Health_data.Clinic_historical;

namespace Clinic_historical_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicHistoricalTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _clinicHistorical = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptClinicHistoricalTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _clinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicHistoricalTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_clinicHistorical);
    }

    [GlobalSetup(Target = nameof(DecryptClinicHistoricalTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedClinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
        _clinicHistorical = _twoFish.Encrypt(generatedClinicHistorical);
    }

    [Benchmark]
    public byte[] DecryptClinicHistoricalTwoFish() => _twoFish.Decrypt(_clinicHistorical);
}