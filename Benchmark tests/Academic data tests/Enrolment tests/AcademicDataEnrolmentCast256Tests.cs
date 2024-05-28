using System.Security.Cryptography;
using System.Text;
using Academic_data.Enrolment;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Enrolment_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataEnrolmentCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _enrolment = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptEnrolmentCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEnrolmentCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_enrolment);
    }

    [GlobalSetup(Target = nameof(DecryptEnrolmentCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedEnrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
        _enrolment = _cast256.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public byte[] DecryptEnrolmentCast256() => _cast256.Decrypt(_enrolment);
}