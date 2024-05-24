using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Afore;

namespace Afore_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _afore = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptAforeTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _afore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAforeTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_afore);
    }

    [GlobalSetup(Target = nameof(DecryptAforeTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedAfore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
        _afore = _twoFish.Encrypt(generatedAfore);
    }

    [Benchmark]
    public byte[] DecryptAforeTwoFish() => _twoFish.Decrypt(_afore);
}