using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Microsoft.Research.SEAL;
using Patrimony_data.Salary;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryCkksTests
{
    private Ckks _ckks = null!;
    private long _salary;
    private Ciphertext? _salaryEncrypted;

    [GlobalSetup(Target = nameof(EncryptSalaryCkks))]
    public void SetupEncryption()
    {
        _ckks = new Ckks();
        _salary = (long)SalaryGenerator.GenerateSalary();
    }

    [Benchmark]
    public Ciphertext EncryptSalaryCkks() => _ckks.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryCkks))]
    public void SetupDecryption()
    {
        _ckks = new Ckks();

        var generatedSalary = (long)SalaryGenerator.GenerateSalary();
        _salaryEncrypted = _ckks.Encrypt(generatedSalary);
    }

    [Benchmark]
    public long DecryptSalaryCkks() => _ckks.Decrypt(_salaryEncrypted!);
}