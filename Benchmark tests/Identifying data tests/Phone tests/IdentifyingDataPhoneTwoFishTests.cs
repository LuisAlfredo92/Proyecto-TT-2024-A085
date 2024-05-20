using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Phone_numbers;

namespace Phone_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataPhoneTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _phone = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptPhoneTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _phone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPhoneTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_phone);
    }

    [GlobalSetup(Target = nameof(DecryptPhonesTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedPhone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        _phone = _twoFish.Encrypt(generatedPhone);
    }

    [Benchmark]
    public byte[] DecryptPhonesTwoFish() => _twoFish.Decrypt(_phone);
}