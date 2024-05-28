using System.Security.Cryptography;
using Academic_data.Enrolment;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;

namespace Enrolment_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataEnrolmentCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _enrolment = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEnrolmentCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _enrolment = EnrolmentGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEnrolmentCamelliaFpe() => _camelliaFpe.Encrypt(_enrolment);

    [GlobalSetup(Target = nameof(DecryptEnrolmentCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedEnrolment = EnrolmentGenerator.Generate().ToCharArray();
        _enrolment = _camelliaFpe.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public char[] DecryptEnrolmentCamelliaFpe() => _camelliaFpe.Decrypt(_enrolment);
}