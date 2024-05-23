using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Vehicular_license;
using Aes = BlockCiphers.Aes;

namespace Vehicular_license_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVehicularLicenseAesTests
{
    private Aes _aes = null!;
    private byte[] _vehicularLicense = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _vehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
    }

    [Benchmark]
    public byte[] EncryptVehicularLicenseAes() => _aes.Encrypt(_vehicularLicense, out _);

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedVehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
        _vehicularLicense = _aes.Encrypt(generatedVehicularLicense, out _tag);
    }

    [Benchmark]
    public byte[] DecryptVehicularLicenseAes() => _aes.Decrypt(_vehicularLicense, _tag);
}