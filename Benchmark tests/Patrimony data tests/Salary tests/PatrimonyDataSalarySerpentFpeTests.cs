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
public class PatrimonyDataSalarySerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _salary = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSalarySerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _salary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSalarySerpentFpe() => _serpentFpe.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalarySerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedSalary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _salary = _serpentFpe.Encrypt(generatedSalary);
    }

    [Benchmark]
    public char[] DecryptSalarySerpentFpe() => _serpentFpe.Decrypt(_salary);
}