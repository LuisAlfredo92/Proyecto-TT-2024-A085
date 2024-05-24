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
public class PatrimonyDataSalarySerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _salary = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptSalarySerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _salary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSalarySerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_salary);
    }

    [GlobalSetup(Target = nameof(DecryptSalarySerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedSalary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
        _salary = _serpent.Encrypt(generatedSalary);
    }

    [Benchmark]
    public byte[] DecryptSalarySerpent() => _serpent.Decrypt(_salary);
}