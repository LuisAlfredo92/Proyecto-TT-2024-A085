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
public class AcademicDataEnrolmentSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _enrolment = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEnrolmentSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _enrolment = EnrolmentGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEnrolmentSerpentFpe() => _serpentFpe.Encrypt(_enrolment);

    [GlobalSetup(Target = nameof(DecryptEnrolmentSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedEnrolment = EnrolmentGenerator.Generate().ToCharArray();
        _enrolment = _serpentFpe.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public char[] DecryptEnrolmentSerpentFpe() => _serpentFpe.Decrypt(_enrolment);
}