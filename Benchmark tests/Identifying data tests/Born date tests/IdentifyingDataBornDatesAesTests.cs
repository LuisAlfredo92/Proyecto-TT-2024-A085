using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Identifying_data.Born_dates;
using Aes = BlockCiphers.Aes;

namespace Born_date_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesAesTests
{
    private Aes _aes = null!;
    private byte[] _bornDate = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptBornDatesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _bornDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public byte[] EncryptBornDatesAes() => _aes.Encrypt(_bornDate, out _);

    [GlobalSetup(Target = nameof(DecryptBornDatesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDate = _aes.Encrypt(generatedDate, out _tag);
    }

    [Benchmark]
    public byte[] DecryptBornDatesAes() => _aes.Decrypt(_bornDate, _tag);
}