using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;
using Transit_and_migratory_data.Vehicular_license;

namespace Vehicular_license_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVehicularLicenseChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _vehicularLicense = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptVehicularLicenseChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _vehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptVehicularLicenseChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_vehicularLicense);
    }

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedVehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
        _vehicularLicense = _chaCha20.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public byte[] DecryptVehicularLicenseChaCha20() => _chaCha20.Decrypt(_vehicularLicense);
}