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
public class IdentifyingDataPhoneCamelliaFpeTests
{
    private CamelliaFpe _serpentFpe = null!;
    private char[] _phone = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPhonesCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _phone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPhonesCamelliaFpe() => _serpentFpe.Encrypt(_phone);

    [GlobalSetup(Target = nameof(DecryptPhonesCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedPhone = PhoneNumbersGenerator.GeneratePhoneNumber().ToString().ToCharArray();
        _phone = _serpentFpe.Encrypt(generatedPhone);
    }

    [Benchmark]
    public char[] DecryptPhonesCamelliaFpe() => _serpentFpe.Decrypt(_phone);
}