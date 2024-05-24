using System.Collections;
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
public class PatrimonyDataCvvGoldwasserMicaliTests
{
    private GoldwasserMicali _goldwasserMicali = null!;
    private byte[] _cvv = null!;
    private BigInteger[]? _cvvEncrypted;
    private GoldwasserMicali.GmKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptCvvGoldwasserMicali))]
    public void SetupEncryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public BigInteger[] EncryptCvvGoldwasserMicali() => _goldwasserMicali.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvGoldwasserMicali))]
    public void SetupDecryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvvEncrypted = _goldwasserMicali.Encrypt(generatedCvv);
    }

    [Benchmark]
    public BitArray DecryptCvvGoldwasserMicali() => _goldwasserMicali.Decrypt(_cvvEncrypted!);
}