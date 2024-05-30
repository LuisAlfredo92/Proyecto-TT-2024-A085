using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Adn;
using BlockCiphers;

namespace Adn_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataAdnSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _adn = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptAdnSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAdnSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_adn);
    }

    [GlobalSetup(Target = nameof(DecryptAdnSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedAdn = AdnGenerator.Generate();
        _adn = _serpent.Encrypt(generatedAdn);
    }

    [Benchmark]
    public byte[] DecryptAdnSerpent() => _serpent.Decrypt(_adn);
}