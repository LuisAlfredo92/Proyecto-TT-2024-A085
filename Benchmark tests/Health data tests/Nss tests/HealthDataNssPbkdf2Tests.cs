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
public class HealthDataNssPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _nss = null!;

    [GlobalSetup(Target = nameof(EncryptNssPbkdf2))]
    public void SetupEncryption()
    {
        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptNssPbkdf2() => _argon2Id.Hash(_nss);
}