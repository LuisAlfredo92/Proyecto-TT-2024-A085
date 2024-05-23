using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Transit_and_migratory_data.Visa;

namespace Visa_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVisaArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _visa = null!;

    [GlobalSetup(Target = nameof(EncryptVisaArgon2Id))]
    public void SetupEncryption()
    {
        _visa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
    }

    [Benchmark]
    public Span<byte> EncryptVisaArgon2Id() => _argon2Id.Hash(_visa);
}