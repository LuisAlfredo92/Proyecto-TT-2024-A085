using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Insurance_identifier_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataInsuranceIdentifierTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _insuranceIdentifier = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptInsuranceIdentifierTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _insuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptInsuranceIdentifierTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_insuranceIdentifier);
    }

    [GlobalSetup(Target = nameof(DecryptInsuranceIdentifierTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedInsuranceIdentifier = BitConverter.GetBytes(Random.Shared.NextInt64());
        _insuranceIdentifier = _twoFish.Encrypt(generatedInsuranceIdentifier);
    }

    [Benchmark]
    public byte[] DecryptInsuranceIdentifierTwoFish() => _twoFish.Decrypt(_insuranceIdentifier);
}