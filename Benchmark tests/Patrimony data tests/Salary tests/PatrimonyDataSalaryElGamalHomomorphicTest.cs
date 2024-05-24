using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Patrimony_data.Salary;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryElGamalHomomorphicTests
{
    private ElGamalHomomorphic _elGamal = null!;
    private BigInteger _salary = null!;
    private (BigInteger yotta, BigInteger delta) _salaryEncrypted;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptSalaryElGamalHomomorphic))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _salary = BigInteger.ValueOf((long)SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public (BigInteger yotta, BigInteger delta) EncryptSalaryElGamalHomomorphic() => _elGamal.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryElGamalHomomorphic))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamalHomomorphic((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedSalary = BigInteger.ValueOf((long)SalaryGenerator.GenerateSalary());
        _salaryEncrypted = _elGamal.Encrypt(generatedSalary);
    }

    [Benchmark]
    public BigInteger DecryptSalaryElGamalHomomorphic() => _elGamal.Decrypt(_salaryEncrypted);
}