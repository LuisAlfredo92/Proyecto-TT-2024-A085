using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Health_data.Clinic_historical;
using Stream_ciphers;

namespace Clinic_historical_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicHistoricalChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _clinicHistorical = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptClinicHistoricalChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _clinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicHistoricalChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_clinicHistorical);
    }

    [GlobalSetup(Target = nameof(DecryptClinicHistoricalChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedClinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
        _clinicHistorical = _chaCha20.Encrypt(generatedClinicHistorical);
    }

    [Benchmark]
    public byte[] DecryptClinicHistoricalChaCha20() => _chaCha20.Decrypt(_clinicHistorical);
}