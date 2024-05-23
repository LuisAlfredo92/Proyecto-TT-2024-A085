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
public class TransitAndMigratoryDataVisaSha2Tests
{
    private byte[] _visa = null!;

    [GlobalSetup(Target = nameof(EncryptVisaSha2))]
    public void SetupEncryption()
    {
        _visa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
    }

    [Benchmark]
    public Span<byte> EncryptVisaSha2() => Sha2.Hash(_visa);
}