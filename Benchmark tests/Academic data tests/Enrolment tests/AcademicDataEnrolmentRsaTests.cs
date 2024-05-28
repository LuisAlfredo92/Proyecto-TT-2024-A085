using System.Security.Cryptography;
using System.Text;
using Academic_data.Enrolment;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;

namespace Enrolment_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataEnrolmentRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _enrolment = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptEnrolmentRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _enrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptEnrolmentRsa() => _rsa.Encrypt(_enrolment);

    [GlobalSetup(Target = nameof(DecryptEnrolmentRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedEnrolment = Encoding.UTF8.GetBytes(EnrolmentGenerator.Generate());
        _enrolment = _rsa.Encrypt(generatedEnrolment);
    }

    [Benchmark]
    public byte[] DecryptEnrolmentRsa() => _rsa.Decrypt(_enrolment);
}