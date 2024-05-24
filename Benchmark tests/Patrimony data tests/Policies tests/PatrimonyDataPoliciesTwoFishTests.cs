using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Policies;

namespace Policies_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataPoliciesTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _policies = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptPoliciesTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _policies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPoliciesTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_policies);
    }

    [GlobalSetup(Target = nameof(DecryptPoliciesTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedPolicies = File.ReadAllBytes(PoliciesGenerator.GeneratePolicy());
        _policies = _twoFish.Encrypt(generatedPolicies);
    }

    [Benchmark]
    public byte[] DecryptPoliciesTwoFish() => _twoFish.Decrypt(_policies);
}