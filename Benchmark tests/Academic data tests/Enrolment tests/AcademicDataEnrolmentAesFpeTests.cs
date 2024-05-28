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
public class AcademicDataEnrolmentAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _enrolment = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEnrolmentAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _enrolment = EnrolmentGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEnrolmentAesFpe() => _aesFpe.Encrypt(_enrolment);

    [GlobalSetup(Target = nameof(DecryptEnrolmentAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedEnrolment = EnrolmentGenerator.Generate().ToCharArray();
        _enrolment = _aesFpe.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public char[] DecryptEnrolmentAesFpe() => _aesFpe.Decrypt(_enrolment);
}