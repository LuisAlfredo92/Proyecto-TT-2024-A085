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
public class TransitAndMigratoryDataVehicularLicenseSha3Tests
{
    private byte[] _vehicularLicense = null!;

    [GlobalSetup(Target = nameof(EncryptVehicularLicenseSha3))]
    public void SetupEncryption()
    {
        _vehicularLicense = Encoding.UTF8.GetBytes(VehicularLicenseGenerator.GenerateVehicularLicense());
    }

    [Benchmark]
    public Span<byte> EncryptVehicularLicenseSha3() => Sha3.Hash(_vehicularLicense);
}