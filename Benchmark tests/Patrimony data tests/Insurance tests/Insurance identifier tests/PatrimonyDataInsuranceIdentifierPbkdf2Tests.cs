using BenchmarkDotNet.Attributes;
using Hashes;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _insuranceIdentifier = null!;

    [GlobalSetup(Target = nameof(EncryptInsuranceIdentifierPbkdf2))]
    public void SetupEncryption()
    {
        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public Span<byte> EncryptInsuranceIdentifierPbkdf2() => _argon2Id.Hash(_insuranceIdentifier);
}