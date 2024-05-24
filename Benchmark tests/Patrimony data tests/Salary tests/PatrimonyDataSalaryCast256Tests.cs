using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Salary;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _salary = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptSalaryCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _salary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSalaryCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_salary);
    }

    [GlobalSetup(Target = nameof(DecryptSalaryCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedSalary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
        _salary = _cast256.Encrypt(generatedSalary);
    }

    [Benchmark]
    public byte[] DecryptSalaryCast256() => _cast256.Decrypt(_salary);
}