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
public class TransitAndMigratoryDataAutomobileLicensePlatePbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _automobileLicensePlate = null!;

    [GlobalSetup(Target = nameof(EncryptAutomobileLicensePlatePbkdf2))]
    public void SetupEncryption()
    {
        _automobileLicensePlate = Encoding.UTF8.GetBytes(AutomobileLicensePlateGenerator.GenerateAutomobileLicensePlate());
    }

    [Benchmark]
    public Span<byte> EncryptAutomobileLicensePlatePbkdf2() => _argon2Id.Hash(_automobileLicensePlate);
}