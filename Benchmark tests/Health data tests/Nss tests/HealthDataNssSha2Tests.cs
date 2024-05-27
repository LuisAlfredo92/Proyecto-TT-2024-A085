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
public class HealthDataNssSha2Tests
{
    private byte[] _nss = null!;

    [GlobalSetup(Target = nameof(EncryptNssSha2))]
    public void SetupEncryption()
    {
        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptNssSha2() => Sha2.Hash(_nss);
}