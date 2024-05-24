using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using General_Data;

namespace Details_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceDetailsTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _insuranceDetails = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptInsuranceDetailsTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _insuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceDetailsTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_insuranceDetails);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceDetailsTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedInsuranceDetails = Encoding.UTF8.GetBytes(StringGenerator.GenerateStringWithSpaces(Random.Shared.Next(256)));
        _insuranceDetails = _twoFish.Encrypt(generatedInsuranceDetails);
    }

    [Benchmark]
    public byte[] DecryptInsuranceDetailsTwoFish() => _twoFish.Decrypt(_insuranceDetails);
}