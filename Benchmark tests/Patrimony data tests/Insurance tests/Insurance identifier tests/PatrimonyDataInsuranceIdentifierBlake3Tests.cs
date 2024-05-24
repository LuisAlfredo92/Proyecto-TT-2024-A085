using BenchmarkDotNet.Attributes;
using Hashes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierBlake3Tests
{
    private byte[] _insuranceIdentifier = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierBlake3))]
    public void SetupEncryption()
    {
        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public Span<byte> EncryptInsuranceIdentifierBlake3() => Blake3.Hash(_insuranceIdentifier);
}