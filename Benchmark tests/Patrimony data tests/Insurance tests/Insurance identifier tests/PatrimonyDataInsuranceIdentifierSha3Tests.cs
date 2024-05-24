using BenchmarkDotNet.Attributes;
using Hashes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierSha3Tests
{
    private byte[] _insuranceIdentifier = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierSha3))]
    public void SetupEncryption()
    {
        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public Span<byte> EncryptInsuranceIdentifierSha3() => Sha3.Hash(_insuranceIdentifier);
}