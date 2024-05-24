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
public class PatrimonyDataCvvCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _cvv = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCvvCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _cvv = CvvGenerator.GenerateCvv().ToString(CultureInfo.InvariantCulture).PadLeft(4, '0').ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCvvCast256Fpe() => _cast256Fpe.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedCvv = CvvGenerator.GenerateCvv().ToString(CultureInfo.InvariantCulture).PadLeft(4, '0').ToCharArray();
        _cvv = _cast256Fpe.Encrypt(generatedCvv);
    }

    [Benchmark]
    public char[] DecryptCvvCast256Fpe() => _cast256Fpe.Decrypt(_cvv);
}