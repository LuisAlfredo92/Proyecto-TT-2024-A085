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
public class TransitAndMigratoryDataVehicularLicenseCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _vehicularLicense = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _vehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVehicularLicenseCamelliaFpe() => _camelliaFpe.Encrypt(_vehicularLicense);

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedVehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
        _vehicularLicense = _camelliaFpe.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public char[] DecryptVehicularLicenseCamelliaFpe() => _camelliaFpe.Decrypt(_vehicularLicense);
}