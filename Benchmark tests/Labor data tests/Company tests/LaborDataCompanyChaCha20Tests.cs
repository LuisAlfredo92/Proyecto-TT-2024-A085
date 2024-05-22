using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using LaborData.Company;
using Stream_ciphers;

namespace Company_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataCompanyChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _company = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptCompanyChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _company = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCompanyChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_company);
    }

    [GlobalSetup(Target = nameof(DecryptCompanyChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedCompany = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
        _company = _chaCha20.Encrypt(generatedCompany);
    }

    [Benchmark]
    public byte[] DecryptCompanyChaCha20() => _chaCha20.Decrypt(_company);
}