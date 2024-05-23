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
public class TransitAndMigratoryDataVehicularLicenseCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _vehicularLicense = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _vehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptVehicularLicenseCast256Fpe() => _cast256Fpe.Encrypt(_vehicularLicense);

    [GlobalSetup(Target = nameof(DecryptVehicularLicenseCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedVehicularLicense = VehicularLicenseGenerator.GenerateVehicularLicense().ToCharArray();
        _vehicularLicense = _cast256Fpe.Encrypt(generatedVehicularLicense);
    }

    [Benchmark]
    public char[] DecryptVehicularLicenseCast256Fpe() => _cast256Fpe.Decrypt(_vehicularLicense);
}