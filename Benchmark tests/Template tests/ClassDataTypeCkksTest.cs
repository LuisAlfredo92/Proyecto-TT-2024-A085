using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Microsoft.Research.SEAL;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeCkksTests
{
    private Ckks _ckks = null!;
    private long _yourData;
    private Ciphertext? _yourDataEncrypted;

    [GlobalSetup(Target = nameof(EncryptTypeCkks))]
    public void SetupEncryption()
    {
        _ckks = new Ckks();
        _yourData = TypeGenerator.GenerateBornDate().Ticks;
    }

    [Benchmark]
    public Ciphertext EncryptTypeCkks() => _ckks.Encrypt(_yourData);

    [GlobalSetup(Target = nameof(DecryptTypeCkks))]
    public void SetupDecryption()
    {
        _ckks = new Ckks();

        var generatedType = TypeGenerator.GenerateBornDate().Ticks;
        _yourDataEncrypted = _ckks.Encrypt(generatedType);
    }

    [Benchmark]
    public long DecryptTypeCkks() => _ckks.Decrypt(_yourDataEncrypted!);
}