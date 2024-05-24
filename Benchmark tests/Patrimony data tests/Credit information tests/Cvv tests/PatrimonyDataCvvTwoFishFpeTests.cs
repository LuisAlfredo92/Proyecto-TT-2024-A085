using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Cvv;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _cvv = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCvvTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _cvv = CvvGenerator.GenerateCvv().ToString(CultureInfo.InvariantCulture).PadLeft(4, '0').ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCvvTwoFishFpe() => _twoFishFpe.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedCvv = CvvGenerator.GenerateCvv().ToString(CultureInfo.InvariantCulture).PadLeft(4, '0').ToCharArray();
        _cvv = _twoFishFpe.Encrypt(generatedCvv);
    }

    [Benchmark]
    public char[] DecryptCvvTwoFishFpe() => _twoFishFpe.Decrypt(_cvv);
}