using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Health_data.Nss;
using Aes = BlockCiphers.Aes;

namespace Nss_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataNssAesTests
{
    private Aes _aes = null!;
    private byte[] _nss = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptNssAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptNssAes() => _aes.Encrypt(_nss, out _);

    [GlobalSetup(Target = nameof(DecryptNssAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedNss = BitConverter.GetBytes(NssGenerator.Generate());
        _nss = _aes.Encrypt(generatedNss, out _tag);
    }

    [Benchmark]
    public byte[] DecryptNssAes() => _aes.Decrypt(_nss, _tag);
}