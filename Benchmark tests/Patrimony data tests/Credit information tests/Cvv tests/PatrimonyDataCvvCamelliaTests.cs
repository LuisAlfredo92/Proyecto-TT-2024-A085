using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Cvv;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _cvv = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptCvvCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCvvCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_cvv);
    }

    [GlobalSetup(Target = nameof(DecryptCvvCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvv = _camellia.Encrypt(generatedCvv);
    }

    [Benchmark]
    public byte[] DecryptCvvCamellia() => _camellia.Decrypt(_cvv);
}