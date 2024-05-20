using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Curps;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Curp_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpElGamalTests
{
    private ElGamal _elGamal = null!;
    private byte[] _curp = null!;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptCurpElGamal))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptCurpElGamal() => _elGamal.Encrypt(_curp);

    [GlobalSetup(Target = nameof(DecryptCurpElGamal))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedDate = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
        _curp = _elGamal.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptCurpElGamal() => _elGamal.Decrypt(_curp);
}