using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Born_dates;

namespace Born_date_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _bornDate = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptBornDatesCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _bornDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptBornDatesCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_bornDate);
    }

    [GlobalSetup(Target = nameof(DecryptBornDatesCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDate = _camellia.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptBornDatesCamellia() => _camellia.Decrypt(_bornDate);
}