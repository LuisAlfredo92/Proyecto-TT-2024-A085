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
public class HealthDataNssSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _nss = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNssSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _nss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNssSerpentFpe() => _serpentFpe.Encrypt(_nss);

    [GlobalSetup(Target = nameof(DecryptNssSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedNss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _nss = _serpentFpe.Encrypt(generatedNss);
    }

    [Benchmark]
    public char[] DecryptNssSerpentFpe() => _serpentFpe.Decrypt(_nss);
}