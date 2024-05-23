using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Vehicular_license;

namespace Vehicular_license_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVehicularLicenseAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _vehicularLicense = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _vehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVehicularLicenseAesFpe() => _aesFpe.Encrypt(_vehicularLicense);

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedVehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
        _vehicularLicense = _aesFpe.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public char[] DecryptVehicularLicenseAesFpe() => _aesFpe.Decrypt(_vehicularLicense);
}