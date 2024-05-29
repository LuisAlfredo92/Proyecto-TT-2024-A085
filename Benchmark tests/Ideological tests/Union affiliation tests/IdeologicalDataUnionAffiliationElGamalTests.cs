using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Ideological_data.Union_affiliation;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Union_affiliation_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataUnionAffiliationElGamalTests
{
    private ElGamal _elGamal = null!;
    private byte[] _unionAffiliation = null!;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptUnionAffiliationElGamal))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _unionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
    }

    [Benchmark]
    public byte[] EncryptUnionAffiliationElGamal() => _elGamal.Encrypt(_unionAffiliation);

    [GlobalSetup(Target = nameof(DecryptUnionAffiliationElGamal))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedUnionAffiliation = BitConverter.GetBytes(UnionAffiliationGenerator.GenerateId());
        _unionAffiliation = _elGamal.Encrypt(generatedUnionAffiliation);
    }

    [Benchmark]
    public byte[] DecryptUnionAffiliationElGamal() => _elGamal.Decrypt(_unionAffiliation);
}