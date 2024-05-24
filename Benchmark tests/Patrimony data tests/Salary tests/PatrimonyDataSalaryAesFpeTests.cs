using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Salary;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _salary = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSalaryAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _salary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSalaryAesFpe() => _aesFpe.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedSalary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _salary = _aesFpe.Encrypt(generatedSalary);
    }

    [Benchmark]
    public char[] DecryptSalaryAesFpe() => _aesFpe.Decrypt(_salary);
}