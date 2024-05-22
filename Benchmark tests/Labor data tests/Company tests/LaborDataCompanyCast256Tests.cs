using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using LaborData.Company;

namespace Company_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataCompanyCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _company = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptCompanyCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _company = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCompanyCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_company);
    }

    [GlobalSetup(Target = nameof(DecryptCompanyCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedCompany = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
        _company = _cast256.Encrypt(generatedCompany);
    }

    [Benchmark]
    public byte[] DecryptCompanyCast256() => _cast256.Decrypt(_company);
}