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
public class IdentifyingDataCurpSha3Tests
{
    private byte[] _curp = null!;

    [GlobalSetup(Target = nameof(EncryptCurpSha3))]
    public void SetupEncryption()
    {
        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpSha3() => Sha3.Hash(_curp);
}