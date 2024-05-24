using BenchmarkDotNet.Attributes;
using Hashes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _insuranceIdentifier = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierBCrypt))]
    public void SetupEncryption()
    {
        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public Span<byte> EncryptInsuranceIdentifierBCrypt() => _argon2Id.Hash(_insuranceIdentifier);
}