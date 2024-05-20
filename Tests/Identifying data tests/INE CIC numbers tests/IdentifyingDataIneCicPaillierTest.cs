using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.INE_CIC_numbers;
using Org.BouncyCastle.Math;

namespace Tests.Identifying_data_tests.INE_CIC_numbers_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicPaillierTests
{
    private Paillier _goldwasserMicali = null!;
    private BigInteger _ineCicNumber = null!;
    private Paillier.PaillierKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptIneCicPaillier))]
    public void SetupEncryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        _ineCicNumber = BigInteger.ValueOf(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public BigInteger EncryptIneCicPaillier() => _goldwasserMicali.Encrypt(_ineCicNumber);

    [GlobalSetup(Target = nameof(DecryptIneCicPaillier))]
    public void SetupDecryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        var generatedDate = BigInteger.ValueOf(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _goldwasserMicali.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptIneCicPaillier() => _goldwasserMicali.Decrypt(_ineCicNumber);
}