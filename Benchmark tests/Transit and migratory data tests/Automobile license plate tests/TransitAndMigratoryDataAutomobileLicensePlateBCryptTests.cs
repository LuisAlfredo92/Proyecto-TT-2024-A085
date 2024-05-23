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
public class TransitAndMigratoryDataAutomobileLicensePlateBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _automobileLicensePlate = null!;

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlateBCrypt))]
    public void SetupEncryption()
    {
        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public Span<byte> EncryptAutomobileLicensePlateBCrypt() => _argon2Id.Hash(_automobileLicensePlate);
}