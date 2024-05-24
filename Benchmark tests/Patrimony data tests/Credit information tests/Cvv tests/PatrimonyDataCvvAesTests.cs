using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Cvv;
using Aes = BlockCiphers.Aes;

namespace Cvv_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvAesTests
{
    private Aes _aes = null!;
    private byte[] _cvv = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptCvvAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public byte[] EncryptCvvAes() => _aes.Encrypt(_cvv, out _);

    [GlobalSetup(Target = nameof(DecryptCvvAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvv = _aes.Encrypt(generatedCvv, out _tag);
    }

    [Benchmark]
    public byte[] DecryptCvvAes() => _aes.Decrypt(_cvv, _tag);
}