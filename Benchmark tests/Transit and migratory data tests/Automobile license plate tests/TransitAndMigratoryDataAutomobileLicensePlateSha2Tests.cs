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
public class TransitAndMigratoryDataAutomobileLicensePlateSha2Tests
{
    private byte[] _automobileLicensePlate = null!;

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateSha2))]
    public void SetupEncryption()
    {
        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public Span<byte> EncryptAutomobileLicensePlateSha2() => Sha2.Hash(_automobileLicensePlate);
}