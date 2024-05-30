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
public class BiometricDataAdnArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _adn = null!;

    [GlobalSetup(Target = nameof(EncryptAdnArgon2Id))]
    public void SetupEncryption()
    {
        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public Span<byte> EncryptAdnArgon2Id() => _argon2Id.Hash(_adn);
}