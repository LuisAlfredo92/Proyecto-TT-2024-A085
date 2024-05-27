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
public class AcademicDataSchoolAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _school = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSchoolAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _school = CctGenerator.GenerateCct().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSchoolAesFpe() => _aesFpe.Encrypt(_school);

    [GlobalSetup(Target = nameof(DecryptSchoolAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedSchool = CctGenerator.GenerateCct().ToCharArray();
        _school = _aesFpe.Encrypt(generatedSchool);
    }

    [Benchmark]
    public char[] DecryptSchoolAesFpe() => _aesFpe.Decrypt(_school);
}