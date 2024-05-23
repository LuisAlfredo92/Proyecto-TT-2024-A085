using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Transit_and_migratory_data.Vehicular_license;

namespace Vehicular_license_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVehicularLicenseArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _vehicularLicense = null!;

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseArgon2Id))]
    public void SetupEncryption()
    {
        _vehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
    }

    [Benchmark]
    public Span<byte> EncryptVehicularLicenseArgon2Id() => _argon2Id.Hash(_vehicularLicense);
}