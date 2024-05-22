using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Identifying_data.INE_CIC_numbers;
using Aes = BlockCiphers.Aes;

namespace Ine_cic_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataIneCicAesTests
{
    private Aes _aes = null!;
    private byte[] _ineCicNumber = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptIneCicAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public byte[] EncryptIneCicAes() => _aes.Encrypt(_ineCicNumber, out _);

    [GlobalSetup(Target = nameof(DecryptIneCicAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedDate = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _aes.Encrypt(generatedDate, out _tag);
    }

    [Benchmark]
    public byte[] DecryptIneCicAes() => _aes.Decrypt(_ineCicNumber, _tag);
}