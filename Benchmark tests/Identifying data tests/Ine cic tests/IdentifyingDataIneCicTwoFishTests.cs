using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.INE_CIC_numbers;

namespace Ine_cic_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataIneCicTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _ineCicNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptIneCicTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptIneCicTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_ineCicNumber);
    }

    [GlobalSetup(Target = nameof(DecryptIneCicTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedDate = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _twoFish.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptIneCicTwoFish() => _twoFish.Decrypt(_ineCicNumber);
}