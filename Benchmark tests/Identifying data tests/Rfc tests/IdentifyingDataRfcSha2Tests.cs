using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Identifying_data.Rfc;

namespace Rfc_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataRfcSha2Tests
{
    private byte[] _rfc = null!;

    [GlobalSetup(Target = nameof(EncryptCurpSha2))]
    public void SetupEncryption()
    {
        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpSha2() => Sha2.Hash(_rfc);
}