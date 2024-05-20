using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Identifying_data.Curps;

namespace Curp_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _curp = null!;

    [GlobalSetup(Target = nameof(EncryptCurpArgon2Id))]
    public void SetupEncryption()
    {
        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpArgon2Id() => _argon2Id.Hash(_curp);
}