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
public class PatrimonyDataCvvSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _cvv = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCvvSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _cvv = CvvGenerator.GenerateCvv().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCvvSerpentFpe() => _serpentFpe.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedCvv = CvvGenerator.GenerateCvv().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0').ToCharArray();
        _cvv = _serpentFpe.Encrypt(generatedCvv);
    }

    [Benchmark]
    public char[] DecryptCvvSerpentFpe() => _serpentFpe.Decrypt(_cvv);
}