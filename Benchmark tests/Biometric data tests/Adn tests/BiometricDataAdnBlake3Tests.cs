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
public class BiometricDataAdnBlake3Tests
{
    private byte[] _adn = null!;

    [GlobalSetup(Target = nameof(EncryptAdnBlake3))]
    public void SetupEncryption()
    {
        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public Span<byte> EncryptAdnBlake3() => Blake3.Hash(_adn);
}