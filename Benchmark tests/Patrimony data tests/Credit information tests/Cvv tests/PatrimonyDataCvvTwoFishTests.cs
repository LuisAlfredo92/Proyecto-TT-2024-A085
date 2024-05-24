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
public class PatrimonyDataCvvTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _cvv = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptCvvTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCvvTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_cvv);
    }

    [GlobalSetup(Target = nameof(DecryptCvvTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvv = _twoFish.Encrypt(generatedCvv);
    }

    [Benchmark]
    public byte[] DecryptCvvTwoFish() => _twoFish.Decrypt(_cvv);
}