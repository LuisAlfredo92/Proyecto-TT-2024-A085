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
public class PatrimonyDataSalaryTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _salary = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789.".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptSalaryTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _salary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptSalaryTwoFishFpe() => _twoFishFpe.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedSalary = SalaryGenerator.GenerateSalary().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _salary = _twoFishFpe.Encrypt(generatedSalary);
    }

    [Benchmark]
    public char[] DecryptSalaryTwoFishFpe() => _twoFishFpe.Decrypt(_salary);
}