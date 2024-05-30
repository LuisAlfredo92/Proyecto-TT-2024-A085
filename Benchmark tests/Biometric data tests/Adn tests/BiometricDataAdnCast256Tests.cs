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
public class BiometricDataAdnCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _adn = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptAdnCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAdnCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_adn);
    }

    [GlobalSetup(Target = nameof(DecryptAdnCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedAdn = AdnGenerator.Generate();
        _adn = _cast256.Encrypt(generatedAdn);
    }

    [Benchmark]
    public byte[] DecryptAdnCast256() => _cast256.Decrypt(_adn);
}