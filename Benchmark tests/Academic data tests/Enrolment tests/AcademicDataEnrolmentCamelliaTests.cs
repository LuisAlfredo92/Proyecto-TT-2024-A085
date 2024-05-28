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
public class AcademicDataEnrolmentCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _enrolment = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptEnrolmentCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEnrolmentCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_enrolment);
    }

    [GlobalSetup(Target = nameof(DecryptEnrolmentCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedEnrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
        _enrolment = _camellia.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public byte[] DecryptEnrolmentCamellia() => _camellia.Decrypt(_enrolment);
}