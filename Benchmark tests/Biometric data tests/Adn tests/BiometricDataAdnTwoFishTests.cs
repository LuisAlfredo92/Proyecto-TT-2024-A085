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
public class BiometricDataAdnTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _adn = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptAdnTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAdnTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_adn);
    }

    [GlobalSetup(Target = nameof(DecryptAdnTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedAdn = AdnGenerator.Generate();
        _adn = _twoFish.Encrypt(generatedAdn);
    }

    [Benchmark]
    public byte[] DecryptAdnTwoFish() => _twoFish.Decrypt(_adn);
}