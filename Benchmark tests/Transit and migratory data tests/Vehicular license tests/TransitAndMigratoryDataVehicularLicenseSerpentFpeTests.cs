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
public class TransitAndMigratoryDataVehicularLicenseSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _vehicularLicense = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _vehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVehicularLicenseSerpentFpe() => _serpentFpe.Encrypt(_vehicularLicense);

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedVehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
        _vehicularLicense = _serpentFpe.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public char[] DecryptVehicularLicenseSerpentFpe() => _serpentFpe.Decrypt(_vehicularLicense);
}