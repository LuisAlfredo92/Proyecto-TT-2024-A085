using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Salary;
using Aes = BlockCiphers.Aes;

namespace Salary_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryAesTests
{
    private Aes _aes = null!;
    private byte[] _salary = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptSalaryAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _salary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public byte[] EncryptSalaryAes() => _aes.Encrypt(_salary, out _);

    [GlobalSetup(Target = nameof(DecryptSalaryAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedSalary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
        _salary = _aes.Encrypt(generatedSalary, out _tag);
    }

    [Benchmark]
    public byte[] DecryptSalaryAes() => _aes.Decrypt(_salary, _tag);
}