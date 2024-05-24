using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Microsoft.Research.SEAL;
using Patrimony_data.Cvv;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvCkksTests
{
    private Ckks _ckks = null!;
    private long _cvv;
    private Ciphertext? _cvvEncrypted;

    [GlobalSetup(Target = nameof(EncryptCvvCkks))]
    public void SetupEncryption()
    {
        _ckks = new Ckks();
        _cvv = CvvGenerator.GenerateCvv();
    }

    [Benchmark]
    public Ciphertext EncryptCvvCkks() => _ckks.Encrypt(_cvv);

    [GlobalSetup(Target = nameof(DecryptCvvCkks))]
    public void SetupDecryption()
    {
        _ckks = new Ckks();

        var generatedCvv = CvvGenerator.GenerateCvv();
        _cvvEncrypted = _ckks.Encrypt(generatedCvv);
    }

    [Benchmark]
    public long DecryptCvvCkks() => _ckks.Decrypt(_cvvEncrypted!);
}