using BenchmarkDotNet.Attributes;
using Biometric_data.Adn;
using Hashes;

namespace Adn_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataAdnSha2Tests
{
    private byte[] _adn = null!;

    [GlobalSetup(Target = nameof(EncryptAdnSha2))]
    public void SetupEncryption()
    {
        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public Span<byte> EncryptAdnSha2() => Sha2.Hash(_adn);
}