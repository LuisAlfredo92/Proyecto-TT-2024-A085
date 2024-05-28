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
public class AcademicDataEnrolmentSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _enrolment = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptEnrolmentSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEnrolmentSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_enrolment);
    }

    [GlobalSetup(Target = nameof(DecryptEnrolmentSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedEnrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
        _enrolment = _serpent.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public byte[] DecryptEnrolmentSerpent() => _serpent.Decrypt(_enrolment);
}