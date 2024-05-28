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
public class AcademicDataEnrolmentTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _enrolment = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptEnrolmentTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _enrolment = EnrolmentGenerator.Generate().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptEnrolmentTwoFishFpe() => _twoFishFpe.Encrypt(_enrolment);

    [GlobalSetup(Target = nameof(DecryptEnrolmentTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedEnrolment = EnrolmentGenerator.Generate().ToCharArray();
        _enrolment = _twoFishFpe.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public char[] DecryptEnrolmentTwoFishFpe() => _twoFishFpe.Decrypt(_enrolment);
}