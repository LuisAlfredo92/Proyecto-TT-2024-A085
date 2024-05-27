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
public class HealthDataClinicHistoricalCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _clinicHistorical = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptClinicHistoricalCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _clinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicHistoricalCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_clinicHistorical);
    }

    [GlobalSetup(Target = nameof(DecryptClinicHistoricalCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedClinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
        _clinicHistorical = _cast256.Encrypt(generatedClinicHistorical);
    }

    [Benchmark]
    public byte[] DecryptClinicHistoricalCast256() => _cast256.Decrypt(_clinicHistorical);
}