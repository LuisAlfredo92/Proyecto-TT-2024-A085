using System.Collections;
using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.Born_dates;
using Org.BouncyCastle.Math;

namespace Born_date_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesGoldwasserMicaliTests
{
    private GoldwasserMicali _goldwasserMicali = null!;
    private byte[] _bornDate = null!;
    private BigInteger[]? _bornDateEncrypted;
    private GoldwasserMicali.GmKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptBornDatesGoldwasserMicali))]
    public void SetupEncryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        _bornDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public BigInteger[] EncryptBornDatesGoldwasserMicali() => _goldwasserMicali.Encrypt(_bornDate);

    [GlobalSetup(Target = nameof(DecryptBornDatesGoldwasserMicali))]
    public void SetupDecryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        var generatedDate = BitConverter.GetBytes(BornDatesGenerator.GenerateBornDate().Ticks);
        _bornDateEncrypted = _goldwasserMicali.Encrypt(generatedDate);
    }

    [Benchmark]
    public BitArray DecryptBornDatesGoldwasserMicali() => _goldwasserMicali.Decrypt(_bornDateEncrypted!);
}