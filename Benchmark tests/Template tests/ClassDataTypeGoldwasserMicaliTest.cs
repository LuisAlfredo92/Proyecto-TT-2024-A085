using System.Collections;
using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Org.BouncyCastle.Math;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeGoldwasserMicaliTests
{
    private GoldwasserMicali _goldwasserMicali = null!;
    private byte[] _yourData = null!;
    private BigInteger[]? _yourDataEncrypted;
    private GoldwasserMicali.GmKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptTypeGoldwasserMicali))]
    public void SetupEncryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        _yourData = BitConverter.GetBytes(TypeGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public BigInteger[] EncryptTypeGoldwasserMicali() => _goldwasserMicali.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeGoldwasserMicali))]
    public void SetupDecryption()
    {
        _key = GoldwasserMicali.GenerateKeys();

        _goldwasserMicali = new GoldwasserMicali(_key.Public, _key.Private);

        var generatedType = BitConverter.GetBytes(TypeGenerator.GenerateBornDate().Ticks);
        _yourDataEncrypted = _goldwasserMicali.Encrypt(generatedType);
    }

    [Benchmark]
    public BitArray DecryptTypeGoldwasserMicali() => _goldwasserMicali.Decrypt(_yourDataEncrypted!);
}