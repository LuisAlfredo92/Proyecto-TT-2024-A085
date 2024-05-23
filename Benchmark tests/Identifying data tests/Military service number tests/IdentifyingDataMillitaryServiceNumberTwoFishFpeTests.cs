using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Identifying_data.Military_service_number;

namespace Military_service_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataMillitaryServiceNumberTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _serviceNumber = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptMillitaryServiceNumberTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _serviceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptMillitaryServiceNumberTwoFishFpe() => _twoFishFpe.Encrypt(_serviceNumber);

    [GlobalSetup(Target = nameof(DecryptMillitaryServiceNumberTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedMillitaryServiceNumber = MilitaryServiceNumbersGenerator.GenerateMilitaryServiceNumber().ToCharArray();
        _serviceNumber = _twoFishFpe.Encrypt(generatedMillitaryServiceNumber);
    }

    [Benchmark]
    public char[] DecryptMillitaryServiceNumberTwoFishFpe() => _twoFishFpe.Decrypt(_serviceNumber);
}