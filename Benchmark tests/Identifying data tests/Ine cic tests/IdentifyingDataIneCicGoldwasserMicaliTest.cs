using System.Collections;
using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.INE_CIC_numbers;
using Org.BouncyCastle.Math;

namespace Ine_cic_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicGoldwasserMicaliTests
{
    private GoldwasserMicali _goldwasserMicali = null!;
    private byte[] _ineCicNumber = null!;
    private BigInteger[]? _ineCicNumberEncrypted;
    private GoldwasserMicali.GmKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptIneCicGoldwasserMicali))]
    public void SetupEncryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public BigInteger[] EncryptIneCicGoldwasserMicali() => _goldwasserMicali.Encrypt(_ineCicNumber);

    [GlobalSetup(Target = nameof(DecryptIneCicGoldwasserMicali))]
    public void SetupDecryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        var generatedDate = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumberEncrypted = _goldwasserMicali.Encrypt(generatedDate);
    }

    [Benchmark]
    public BitArray DecryptIneCicGoldwasserMicali() => _goldwasserMicali.Decrypt(_ineCicNumberEncrypted!);
}