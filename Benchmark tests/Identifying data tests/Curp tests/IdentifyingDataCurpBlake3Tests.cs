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
public class IdentifyingDataCurpBlake3Tests
{
    private byte[] _curp = null!;

    [GlobalSetup(Target = nameof(EncryptCurpBlake3))]
    public void SetupEncryption()
    {
        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpBlake3() => Blake3.Hash(_curp);
}