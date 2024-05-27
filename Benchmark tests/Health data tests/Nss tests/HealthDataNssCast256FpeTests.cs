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
public class HealthDataNssCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _nss = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNssCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _nss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNssCast256Fpe() => _cast256Fpe.Encrypt(_nss);

    [GlobalSetup(Target = nameof(DecryptNssCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedNss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _nss = _cast256Fpe.Encrypt(generatedNss);
    }

    [Benchmark]
    public char[] DecryptNssCast256Fpe() => _cast256Fpe.Decrypt(_nss);
}