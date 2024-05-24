using System.Collections;
using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Math;
using Patrimony_data.Salary;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryGoldwasserMicaliTests
{
    private GoldwasserMicali _goldwasserMicali = null!;
    private byte[] _salary = null!;
    private BigInteger[]? _salaryEncrypted;
    private GoldwasserMicali.GmKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptSalaryGoldwasserMicali))]
    public void SetupEncryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        _salary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public BigInteger[] EncryptSalaryGoldwasserMicali() => _goldwasserMicali.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryGoldwasserMicali))]
    public void SetupDecryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        var generatedSalary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
        _salaryEncrypted = _goldwasserMicali.Encrypt(generatedSalary);
    }

    [Benchmark]
    public BitArray DecryptSalaryGoldwasserMicali() => _goldwasserMicali.Decrypt(_salaryEncrypted!);
}