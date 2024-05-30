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
public class BiometricDataAdnPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _adn = null!;

    [GlobalSetup(Target = nameof(EncryptAdnPbkdf2))]
    public void SetupEncryption()
    {
        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public Span<byte> EncryptAdnPbkdf2() => _argon2Id.Hash(_adn);
}