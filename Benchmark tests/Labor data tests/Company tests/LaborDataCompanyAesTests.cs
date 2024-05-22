using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using LaborData.Company;
using Aes = BlockCiphers.Aes;

namespace Company_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataCompanyAesTests
{
    private Aes _aes = null!;
    private byte[] _company = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptCompanyAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _company = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
    }

    [Benchmark]
    public byte[] EncryptCompanyAes() => _aes.Encrypt(_company, out _);

    [GlobalSetup(Target = nameof(DecryptCompanyAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedCompany = Encoding.UTF8.GetBytes(CompanyGenerator.GenerateCompanyName());
        _company = _aes.Encrypt(generatedCompany, out _tag);
    }

    [Benchmark]
    public byte[] DecryptCompanyAes() => _aes.Decrypt(_company, _tag);
}