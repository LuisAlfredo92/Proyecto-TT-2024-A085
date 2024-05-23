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
public class TransitAndMigratoryDataNivArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _niv = null!;

    [GlobalSetup(Target = nameof(EncryptNivArgon2Id))]
    public void SetupEncryption()
    {
        _niv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
    }

    [Benchmark]
    public Span<byte> EncryptNivArgon2Id() => _argon2Id.Hash(_niv);
}