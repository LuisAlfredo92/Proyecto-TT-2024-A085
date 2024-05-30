using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Adn;
using Aes = BlockCiphers.Aes;

namespace Adn_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataAdnAesTests
{
    private Aes _aes = null!;
    private byte[] _adn = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptAdnAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _adn = AdnGenerator.Generate();
    }

    [Benchmark]
    public byte[] EncryptAdnAes() => _aes.Encrypt(_adn, out _);

    [GlobalSetup(Target = nameof(DecryptAdnAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedAdn = AdnGenerator.Generate();
        _adn = _aes.Encrypt(generatedAdn, out _tag);
    }

    [Benchmark]
    public byte[] DecryptAdnAes() => _aes.Decrypt(_adn, _tag);
}