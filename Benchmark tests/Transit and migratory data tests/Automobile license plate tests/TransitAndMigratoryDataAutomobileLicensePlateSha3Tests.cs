using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Transit_and_migratory_data.Automobile_license_plate;

namespace Automobile_license_plate_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataAutomobileLicensePlateSha3Tests
{
    private byte[] _automobileLicensePlate = null!;

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateSha3))]
    public void SetupEncryption()
    {
        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public Span<byte> EncryptAutomobileLicensePlateSha3() => Sha3.Hash(_automobileLicensePlate);
}