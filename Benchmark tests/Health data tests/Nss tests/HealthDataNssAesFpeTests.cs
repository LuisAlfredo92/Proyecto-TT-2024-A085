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
public class HealthDataNssAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _nss = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNssAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _nss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNssAesFpe() => _aesFpe.Encrypt(_nss);

    [GlobalSetup(Target = nameof(DecryptNssAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedNss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _nss = _aesFpe.Encrypt(generatedNss);
    }

    [Benchmark]
    public char[] DecryptNssAesFpe() => _aesFpe.Decrypt(_nss);
}