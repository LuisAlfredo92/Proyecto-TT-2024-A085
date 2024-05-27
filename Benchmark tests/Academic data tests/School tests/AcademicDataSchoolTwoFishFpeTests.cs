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
public class AcademicDataSchoolTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _school = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSchoolTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _school = CctGenerator.GenerateCct().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSchoolTwoFishFpe() => _twoFishFpe.Encrypt(_school);

    [GlobalSetup(Target = nameof(DecryptSchoolTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedSchool = CctGenerator.GenerateCct().ToCharArray();
        _school = _twoFishFpe.Encrypt(generatedSchool);
    }

    [Benchmark]
    public char[] DecryptSchoolTwoFishFpe() => _twoFishFpe.Decrypt(_school);
}