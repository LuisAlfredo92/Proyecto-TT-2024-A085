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
public class IdentifyingDataIneCicCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _ineCicNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptNamesCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_ineCicNumber);
    }

    [GlobalSetup(Target = nameof(DecryptNamesCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedDate = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _camellia.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptNamesCamellia() => _camellia.Decrypt(_ineCicNumber);
}