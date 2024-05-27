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
public class HealthDataNssBlake3Tests
{
    private byte[] _nss = null!;

    [GlobalSetup(Target = nameof(EncryptNssBlake3))]
    public void SetupEncryption()
    {
        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public Span<byte> EncryptNssBlake3() => Blake3.Hash(_nss);
}