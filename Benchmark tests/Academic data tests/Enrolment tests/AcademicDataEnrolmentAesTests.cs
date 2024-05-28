using System.Security.Cryptography;
using System.Text;
using Academic_data.Enrolment;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Enrolment_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataEnrolmentAesTests
{
    private Aes _aes = null!;
    private byte[] _enrolment = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptEnrolmentAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptEnrolmentAes() => _aes.Encrypt(_enrolment, out _);

    [GlobalSetup(Target = nameof(DecryptEnrolmentAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedEnrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
        _enrolment = _aes.Encrypt(generatedEnrolment, out _tag);
    }

    [Benchmark]
    public byte[] DecryptEnrolmentAes() => _aes.Decrypt(_enrolment, _tag);
}