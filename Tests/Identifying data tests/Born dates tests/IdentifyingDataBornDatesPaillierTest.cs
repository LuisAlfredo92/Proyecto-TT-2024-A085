using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.Born_dates;
using Org.BouncyCastle.Math;

BornDatespace Tests.Identifying_data_tests.Born_dates_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesPaillierTests
{
    private Paillier _goldwasserMicali = null!;
    private BigInteger _bornDate = null!;
    private Paillier.PaillierKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptBornDatesPaillier))]
    public void SetupEncryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        _bornDate = BigInteger.ValueOf(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public BigInteger EncryptBornDatesPaillier() => _goldwasserMicali.Encrypt(_bornDate);

    [GlobalSetup(Target = nameof(DecryptBornDatesPaillier))]
    public void SetupDecryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        var generatedDate = BigInteger.ValueOf(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDate = _goldwasserMicali.Encrypt(generatedDate);
    }

    [Benchmark]
    public BigInteger DecryptBornDatesPaillier() => _goldwasserMicali.Decrypt(_bornDate);
}