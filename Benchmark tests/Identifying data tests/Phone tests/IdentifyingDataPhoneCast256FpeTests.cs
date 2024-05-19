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
public class IdentifyingDataPhoneCast256FpeTests
{
    private Cast256Fpe _serpentFpe = null!;
    private char[] _phone = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPhonesCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _phone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPhonesCast256Fpe() => _serpentFpe.Encrypt(_phone);

    [GlobalSetup(Target = nameof(DecryptPhonesCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedPhone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
        _phone = _serpentFpe.Encrypt(generatedPhone);
    }

    [Benchmark]
    public char[] DecryptPhonesCast256Fpe() => _serpentFpe.Decrypt(_phone);
}