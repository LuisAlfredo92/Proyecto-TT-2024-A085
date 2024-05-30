using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Iris;
using Aes = BlockCiphers.Aes;

namespace Iris_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataIrisAesTests
{
    private Aes _aes = null!;
    private byte[] _iris = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptIrisAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _iris = File.ReadAllBytes(IrisGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptIrisAes() => _aes.Encrypt(_iris, out _);

    [GlobalSetup(Target = nameof(DecryptIrisAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedIris = File.ReadAllBytes(IrisGenerator.Generate());
        _iris = _aes.Encrypt(generatedIris, out _tag);
    }

    [Benchmark]
    public byte[] DecryptIrisAes() => _aes.Decrypt(_iris, _tag);
}