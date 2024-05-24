using BenchmarkDotNet.Attributes;
using Hashes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _insuranceIdentifier = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierArgon2Id))]
    public void SetupEncryption()
    {
        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public Span<byte> EncryptInsuranceIdentifierArgon2Id() => _argon2Id.Hash(_insuranceIdentifier);
}