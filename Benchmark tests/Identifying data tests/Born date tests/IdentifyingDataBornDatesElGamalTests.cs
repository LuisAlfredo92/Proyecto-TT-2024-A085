using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Born_dates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Born_date_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesElGamalTests
{
    private ElGamal _elGamal = null!;
    private byte[] _bornDate = null!;
    private AsymmetricCipherKeyPair? _key;
    private ElGamalParametersGenerator? _parGen;
    private ElGamalParameters? _elParams;
    private ElGamalKeyPairGenerator? _pGen;

    [GlobalSetup(Target = nameof(EncryptBornDatesElGamal))]
    public void SetupEncryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        _bornDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public byte[] EncryptBornDatesElGamal() => _elGamal.Encrypt(_bornDate);

    [GlobalSetup(Target = nameof(DecryptBornDatesElGamal))]
    public void SetupDecryption()
    {
        _parGen = new ElGamalParametersGenerator();
        _parGen.Init(256, 10, new SecureRandom());
        _elParams = _parGen.GenerateParameters();
        _pGen = new ElGamalKeyPairGenerator();
        _pGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), _elParams));
        _key = _pGen.GenerateKeyPair();

        _elGamal = new ElGamal((ElGamalPublicKeyParameters)_key.Public, (ElGamalPrivateKeyParameters)_key.Private);

        var generatedDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDate = _elGamal.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptBornDatesElGamal() => _elGamal.Decrypt(_bornDate);
}