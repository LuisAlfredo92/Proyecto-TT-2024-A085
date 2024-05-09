using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Microsoft.Research.SEAL;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
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

        var generatedDate = TypeGenerator.GenerateBornDate().Ticks;
        _yourDataEncrypted = _ckks.Encrypt(generatedDate);
    }

    [Benchmark]
    public long DecryptTypeCkks() => _ckks.Decrypt(_yourDataEncrypted!);
}