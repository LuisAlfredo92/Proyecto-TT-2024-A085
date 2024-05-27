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
public class AcademicDataSchoolCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _school = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSchoolCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _school = CctGenerator.GenerateCct().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSchoolCamelliaFpe() => _camelliaFpe.Encrypt(_school);

    [GlobalSetup(Target = nameof(DecryptSchoolCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedSchool = CctGenerator.GenerateCct().ToCharArray();
        _school = _camelliaFpe.Encrypt(generatedSchool);
    }

    [Benchmark]
    public char[] DecryptSchoolCamelliaFpe() => _camelliaFpe.Decrypt(_school);
}