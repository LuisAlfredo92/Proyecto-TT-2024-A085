using BenchmarkDotNet.Attributes;
using Hashes;
using Health_data.Nss;

namespace Nss_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataNssArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _nss = null!;

    [GlobalSetup(Target = nameof(EncryptNssArgon2Id))]
    public void SetupEncryption()
    {
        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptNssArgon2Id() => _argon2Id.Hash(_nss);
}