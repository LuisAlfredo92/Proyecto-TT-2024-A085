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
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class IdentifyingDataPhoneSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _phone = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptPhoneSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _phone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPhoneSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_phone);
    }

    [GlobalSetup(Target = nameof(DecryptPhoneSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedPhone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        _phone = _serpent.Encrypt(generatedPhone);
    }

    [Benchmark]
    public byte[] DecryptPhoneSerpent() => _serpent.Decrypt(_phone);
}