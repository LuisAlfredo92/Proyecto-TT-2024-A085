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
public class PatrimonyDataSalaryPaillierTests
{
    private Paillier _goldwasserMicali = null!;
    private BigInteger _salary = null!;
    private Paillier.PaillierKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptSalaryPaillier))]
    public void SetupEncryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        _salary = BigInteger.ValueOf((long)SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public BigInteger EncryptSalaryPaillier() => _goldwasserMicali.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryPaillier))]
    public void SetupDecryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        var generatedSalary = BigInteger.ValueOf((long)SalaryGenerator.GenerateSalary());
        _salary = _goldwasserMicali.Encrypt(generatedSalary);
    }

    [Benchmark]
    public BigInteger DecryptSalaryPaillier() => _goldwasserMicali.Decrypt(_salary);
}