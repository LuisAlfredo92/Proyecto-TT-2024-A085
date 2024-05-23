using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Transit_and_migratory_data.Niv;

namespace Niv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataNivSha2Tests
{
    private byte[] _niv = null!;

    [GlobalSetup(Target = nameof(EncryptNivSha2))]
    public void SetupEncryption()
    {
        _niv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
    }

    [Benchmark]
    public Span<byte> EncryptNivSha2() => Sha2.Hash(_niv);
}