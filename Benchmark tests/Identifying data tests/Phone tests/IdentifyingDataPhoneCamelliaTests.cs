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
public class IdentifyingDataPhoneCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _phone = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptPhonesCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _phone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPhonesCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_phone);
    }

    [GlobalSetup(Target = nameof(DecryptPhonesCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedPhone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        _phone = _camellia.Encrypt(generatedPhone);
    }

    [Benchmark]
    public byte[] DecryptPhonesCamellia() => _camellia.Decrypt(_phone);
}