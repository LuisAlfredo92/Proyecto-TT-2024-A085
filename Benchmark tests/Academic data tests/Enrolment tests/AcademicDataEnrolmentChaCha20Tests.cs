using System.Security.Cryptography;
using System.Text;
using Academic_data.Enrolment;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Enrolment_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataEnrolmentChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _enrolment = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptEnrolmentChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEnrolmentChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_enrolment);
    }

    [GlobalSetup(Target = nameof(DecryptEnrolmentChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedEnrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
        _enrolment = _chaCha20.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public byte[] DecryptEnrolmentChaCha20() => _chaCha20.Decrypt(_enrolment);
}