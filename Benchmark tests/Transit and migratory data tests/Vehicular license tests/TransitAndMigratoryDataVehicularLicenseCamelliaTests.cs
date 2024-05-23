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
public class TransitAndMigratoryDataVehicularLicenseCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _vehicularLicense = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptVehicularLicenseCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _vehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptVehicularLicenseCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_vehicularLicense);
    }

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedVehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
        _vehicularLicense = _camellia.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public byte[] DecryptVehicularLicenseCamellia() => _camellia.Decrypt(_vehicularLicense);
}