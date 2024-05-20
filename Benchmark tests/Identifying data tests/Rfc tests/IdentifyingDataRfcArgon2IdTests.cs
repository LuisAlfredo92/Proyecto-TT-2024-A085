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
public class IdentifyingDataRfcArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _rfc = null!;

    [GlobalSetup(Target = nameof(EncryptCurpArgon2Id))]
    public void SetupEncryption()
    {
        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptCurpArgon2Id() => _argon2Id.Hash(_rfc);
}