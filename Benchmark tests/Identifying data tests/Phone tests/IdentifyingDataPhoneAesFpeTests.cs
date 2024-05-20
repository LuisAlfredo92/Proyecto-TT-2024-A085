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
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataPhoneAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _phone = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPhonesAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _phone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPhonesAesFpe() => _aesFpe.Encrypt(_phone);

    [GlobalSetup(Target = nameof(DecryptPhonesAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedPhone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
        _phone = _aesFpe.Encrypt(generatedPhone);
    }

    [Benchmark]
    public char[] DecryptPhonesAesFpe() => _aesFpe.Decrypt(_phone);
}