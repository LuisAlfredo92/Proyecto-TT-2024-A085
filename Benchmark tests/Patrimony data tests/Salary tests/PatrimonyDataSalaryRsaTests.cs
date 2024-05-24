using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Salary;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _salary = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptSalaryRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _salary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public byte[] EncryptSalaryRsa() => _rsa.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedSalary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
        _salary = _rsa.Encrypt(generatedSalary);
    }

    [Benchmark]
    public byte[] DecryptSalaryRsa() => _rsa.Decrypt(_salary);
}