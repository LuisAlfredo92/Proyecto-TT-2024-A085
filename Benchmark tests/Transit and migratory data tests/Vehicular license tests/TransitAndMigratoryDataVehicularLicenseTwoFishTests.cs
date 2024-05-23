using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Transit_and_migratory_data.Vehicular_license;

namespace Vehicular_license_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVehicularLicenseTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _vehicularLicense = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptVehicularLicenseTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _vehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptVehicularLicenseTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_vehicularLicense);
    }

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedVehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
        _vehicularLicense = _twoFish.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public byte[] DecryptVehicularLicenseTwoFish() => _twoFish.Decrypt(_vehicularLicense);
}