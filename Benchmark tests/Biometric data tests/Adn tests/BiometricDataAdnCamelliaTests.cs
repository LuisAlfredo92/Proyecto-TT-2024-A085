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
public class BiometricDataAdnCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _adn = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptAdnCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAdnCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_adn);
    }

    [GlobalSetup(Target = nameof(DecryptAdnCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedAdn = AdnGenerator.Generate();
        _adn = _camellia.Encrypt(generatedAdn);
    }

    [Benchmark]
    public byte[] DecryptAdnCamellia() => _camellia.Decrypt(_adn);
}