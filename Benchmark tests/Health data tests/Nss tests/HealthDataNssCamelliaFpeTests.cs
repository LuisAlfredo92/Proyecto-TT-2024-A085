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
public class HealthDataNssCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _nss = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptNssCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _nss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptNssCamelliaFpe() => _camelliaFpe.Encrypt(_nss);

    [GlobalSetup(Target = nameof(DecryptNssCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedNss = NssGenerator.Generate().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _nss = _camelliaFpe.Encrypt(generatedNss);
    }

    [Benchmark]
    public char[] DecryptNssCamelliaFpe() => _camelliaFpe.Decrypt(_nss);
}