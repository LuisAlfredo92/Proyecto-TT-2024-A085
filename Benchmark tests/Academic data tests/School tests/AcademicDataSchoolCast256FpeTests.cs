using System.Security.Cryptography;
using Academic_data.Cct;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;

namespace School_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataSchoolCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _school = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSchoolCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _school = CctGenerator.GenerateCct().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSchoolCast256Fpe() => _cast256Fpe.Encrypt(_school);

    [GlobalSetup(Target = nameof(DecryptSchoolCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedSchool = CctGenerator.GenerateCct().ToCharArray();
        _school = _cast256Fpe.Encrypt(generatedSchool);
    }

    [Benchmark]
    public char[] DecryptSchoolCast256Fpe() => _cast256Fpe.Decrypt(_school);
}