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
public class AcademicDataSchoolSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _school = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSchoolSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _school = CctGenerator.GenerateCct().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSchoolSerpentFpe() => _serpentFpe.Encrypt(_school);

    [GlobalSetup(Target = nameof(DecryptSchoolSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedSchool = CctGenerator.GenerateCct().ToCharArray();
        _school = _serpentFpe.Encrypt(generatedSchool);
    }

    [Benchmark]
    public char[] DecryptSchoolSerpentFpe() => _serpentFpe.Decrypt(_school);
}