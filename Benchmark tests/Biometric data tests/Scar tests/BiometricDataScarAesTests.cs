using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Scars;
using Aes = BlockCiphers.Aes;

namespace Scar_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataScarAesTests
{
    private Aes _aes = null!;
    private byte[] _scar = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptScarAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _scar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
    }

    [Benchmark]
    public byte[] EncryptScarAes() => _aes.Encrypt(_scar, out _);

    [GlobalSetup(Target = nameof(DecryptScarAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedScar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
        _scar = _aes.Encrypt(generatedScar, out _tag);
    }

    [Benchmark]
    public byte[] DecryptScarAes() => _aes.Decrypt(_scar, _tag);
}