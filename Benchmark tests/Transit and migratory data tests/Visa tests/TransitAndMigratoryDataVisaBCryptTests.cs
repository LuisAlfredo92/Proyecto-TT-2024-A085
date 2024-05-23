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
public class TransitAndMigratoryDataVisaBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _visa = null!;

    [GlobalSetup(Target = nameof(EncryptVisaBCrypt))]
    public void SetupEncryption()
    {
        _visa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
    }

    [Benchmark]
    public Span<byte> EncryptVisaBCrypt() => _argon2Id.Hash(_visa);
}