using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Adn;
using Stream_ciphers;

namespace Adn_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataAdnChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _adn = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptAdnChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAdnChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_adn);
    }

    [GlobalSetup(Target = nameof(DecryptAdnChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedAdn = AdnGenerator.Generate();
        _adn = _chaCha20.Encrypt(generatedAdn);
    }

    [Benchmark]
    public byte[] DecryptAdnChaCha20() => _chaCha20.Decrypt(_adn);
}