using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.INE_CIC_numbers;

namespace Tests.Identifying_data_tests.INE_CIC_numbers_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _ineCicNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptNamesCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_ineCicNumber);
    }

    [GlobalSetup(Target = nameof(DecryptNamesCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedDate = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _cast256.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptNamesCast256() => _cast256.Decrypt(_ineCicNumber);
}