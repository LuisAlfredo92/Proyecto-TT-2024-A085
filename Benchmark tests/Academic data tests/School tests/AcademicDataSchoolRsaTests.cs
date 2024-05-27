using System.Security.Cryptography;
using System.Text;
using Academic_data.Cct;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;

namespace School_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class AcademicDataSchoolRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _school = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptSchoolRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _school = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
    }

    [Benchmark]
    public byte[] EncryptSchoolRsa() => _rsa.Encrypt(_school);

    [GlobalSetup(Target = nameof(DecryptSchoolRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedSchool = Encoding.UTF8.GetBytes(CctGenerator.GenerateCct());
        _school = _rsa.Encrypt(generatedSchool);
    }

    [Benchmark]
    public byte[] DecryptSchoolRsa() => _rsa.Decrypt(_school);
}