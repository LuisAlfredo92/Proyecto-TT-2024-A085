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
public class AcademicDataEnrolmentCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _enrolment = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEnrolmentCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _enrolment = EnrolmentGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEnrolmentCast256Fpe() => _cast256Fpe.Encrypt(_enrolment);

    [GlobalSetup(Target = nameof(DecryptEnrolmentCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedEnrolment = EnrolmentGenerator.Generate().ToCharArray();
        _enrolment = _cast256Fpe.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public char[] DecryptEnrolmentCast256Fpe() => _cast256Fpe.Decrypt(_enrolment);
}