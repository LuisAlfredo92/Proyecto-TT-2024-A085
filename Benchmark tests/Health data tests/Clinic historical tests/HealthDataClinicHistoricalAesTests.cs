using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Health_data.Clinic_historical;
using Aes = BlockCiphers.Aes;

namespace Clinic_historical_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicHistoricalAesTests
{
    private Aes _aes = null!;
    private byte[] _clinicHistorical = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptClinicHistoricalAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _clinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
    }

    [Benchmark]
    public byte[] EncryptClinicHistoricalAes() => _aes.Encrypt(_clinicHistorical, out _);

    [GlobalSetup(Target = nameof(DecryptClinicHistoricalAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedClinicHistorical = File.ReadAllBytes(ClinicHistoricalGenerator.GenerateStudies());
        _clinicHistorical = _aes.Encrypt(generatedClinicHistorical, out _tag);
    }

    [Benchmark]
    public byte[] DecryptClinicHistoricalAes() => _aes.Decrypt(_clinicHistorical, _tag);
}