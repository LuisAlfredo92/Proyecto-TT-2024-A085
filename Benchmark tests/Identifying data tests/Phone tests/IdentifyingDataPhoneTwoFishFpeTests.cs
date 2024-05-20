using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Phone_numbers;

namespace Phone_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class IdentifyingDataPhoneTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _phone = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPhonesTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _phone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPhonesTwoFishFpe() => _twoFishFpe.Encrypt(_phone);

    [GlobalSetup(Target = nameof(DecryptPhonesTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedPhone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
        _phone = _twoFishFpe.Encrypt(generatedPhone);
    }

    [Benchmark]
    public char[] DecryptPhonesTwoFishFpe() => _twoFishFpe.Decrypt(_phone);
}