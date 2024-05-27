using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Health_data.Nss;

namespace Nss_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataNssTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _nss = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNssTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _nss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNssTwoFishFpe() => _twoFishFpe.Encrypt(_nss);

    [GlobalSetup(Target = nameof(DecryptNssTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedNss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _nss = _twoFishFpe.Encrypt(generatedNss);
    }

    [Benchmark]
    public char[] DecryptNssTwoFishFpe() => _twoFishFpe.Decrypt(_nss);
}