using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Math;
using Patrimony_data.Cvv;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvPaillierTests
{
    private Paillier _goldwasserMicali = null!;
    private BigInteger _cvv = null!;
    private Paillier.PaillierKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptCvvPaillier))]
    public void SetupEncryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        _cvv = BigInteger.ValueOf(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public BigInteger EncryptCvvPaillier() => _goldwasserMicali.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvPaillier))]
    public void SetupDecryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        var generatedCvv = BigInteger.ValueOf(CvvGenerator.GenerateCvv());
        _cvv = _goldwasserMicali.Encrypt(generatedCvv);
    }

    [Benchmark]
    public BigInteger DecryptCvvPaillier() => _goldwasserMicali.Decrypt(_cvv);
}