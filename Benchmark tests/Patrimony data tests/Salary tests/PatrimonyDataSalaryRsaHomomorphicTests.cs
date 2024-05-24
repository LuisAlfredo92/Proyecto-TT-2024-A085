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
public class PatrimonyDataSalaryRsaHomomorphicTests
{
    private RsaHomomorphic _rsa = null!;
    private BigInteger _salary = null!;
    private RsaKeyPairGenerator? _pGen;
    private AsymmetricCipherKeyPair? _key;

    [GlobalSetup(Target = nameof(EncryptSalaryRsaHomomorphic))]
    public void SetupEncryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        _salary = BigInteger.ValueOf((long)SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public BigInteger EncryptSalaryRsaHomomorphic() => _rsa.Encrypt(_salary);

    [GlobalSetup(Target = nameof(DecryptSalaryRsaHomomorphic))]
    public void SetupDecryption()
    {
        _pGen = new RsaKeyPairGenerator();
        _pGen.Init(new KeyGenerationParameters(new SecureRandom(), 256));
        _key = _pGen.GenerateKeyPair();

        _rsa = new RsaHomomorphic((_key.Public as RsaKeyParameters)!, (_key.Private as RsaKeyParameters)!);

        var generatedSalary = BigInteger.ValueOf((long)SalaryGenerator.GenerateSalary());
        _salary = _rsa.Encrypt(generatedSalary);
    }

    [Benchmark]
    public BigInteger DecryptSalaryRsaHomomorphic() => _rsa.Decrypt(_salary);
}