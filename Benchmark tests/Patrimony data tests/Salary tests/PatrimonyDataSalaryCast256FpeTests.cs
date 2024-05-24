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
public class PatrimonyDataSalaryCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _salary = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSalaryCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _salary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSalaryCast256Fpe() => _cast256Fpe.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedSalary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _salary = _cast256Fpe.Encrypt(generatedSalary);
    }

    [Benchmark]
    public char[] DecryptSalaryCast256Fpe() => _cast256Fpe.Decrypt(_salary);
}