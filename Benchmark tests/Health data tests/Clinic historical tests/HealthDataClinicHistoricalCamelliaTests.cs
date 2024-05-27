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
public class HealthDataClinicHistoricalCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _clinicHistorical = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptClinicHistoricalCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _clinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicHistoricalCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_clinicHistorical);
    }

    [GlobalSetup(Target = nameof(DecryptClinicHistoricalCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedClinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
        _clinicHistorical = _camellia.Encrypt(generatedClinicHistorical);
    }

    [Benchmark]
    public byte[] DecryptClinicHistoricalCamellia() => _camellia.Decrypt(_clinicHistorical);
}