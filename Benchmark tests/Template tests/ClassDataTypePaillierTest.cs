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
public class ClassDataTypePaillierTests
{
    private Paillier _goldwasserMicali = null!;
    private BigInteger _yourData = null!;
    private Paillier.PaillierKeyPair _key;

    [GlobalSetup(Target = nameof(EncryptTypePaillier))]
    public void SetupEncryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        _yourData = BigInteger.ValueOf(TypeGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public BigInteger EncryptTypePaillier() => _goldwasserMicali.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypePaillier))]
    public void SetupDecryption()
    {
        _key = Paillier.GenerateKeys();

        _goldwasserMicali = new Paillier(_key.Public, _key.Private);

        var generatedType = BigInteger.ValueOf(TypeGenerator.GenerateBornDate().Ticks);
        _yourData = _goldwasserMicali.Encrypt(generatedType);
    }

    [Benchmark]
    public BigInteger DecryptTypePaillier() => _goldwasserMicali.Decrypt(_yourData);
}