using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Health_data.Clinic_studies;
using Stream_ciphers;

namespace Clinic_studies_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataClinicStudiesChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _clinicStudies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptClinicStudiesChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _clinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptClinicStudiesChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_clinicStudies);
    }

    [GlobalSetup(Target = nameof(DecryptClinicStudiesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedClinicStudies = File.ReadAllBytes(ClinicStudiesGenerator.Generate());
        _clinicStudies = _chaCha20.Encrypt(generatedClinicStudies);
    }

    [Benchmark]
    public byte[] DecryptClinicStudiesChaCha20() => _chaCha20.Decrypt(_clinicStudies);
}