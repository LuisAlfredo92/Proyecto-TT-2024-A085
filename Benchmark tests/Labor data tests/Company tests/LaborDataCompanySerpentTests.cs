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
public class LaborDataCompanySerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _company = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptCompanySerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _company = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCompanySerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_company);
    }

    [GlobalSetup(Target = nameof(DecryptCompanySerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedCompany = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
        _company = _serpent.Encrypt(generatedCompany);
    }

    [Benchmark]
    public byte[] DecryptCompanySerpent() => _serpent.Decrypt(_company);
}