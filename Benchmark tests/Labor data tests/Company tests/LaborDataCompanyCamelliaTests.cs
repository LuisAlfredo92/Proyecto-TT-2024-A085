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
public class LaborDataCompanyCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _company = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptCompanyCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _company = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCompanyCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_company);
    }

    [GlobalSetup(Target = nameof(DecryptCompanyCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedCompany = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
        _company = _camellia.Encrypt(generatedCompany);
    }

    [Benchmark]
    public byte[] DecryptCompanyCamellia() => _camellia.Decrypt(_company);
}