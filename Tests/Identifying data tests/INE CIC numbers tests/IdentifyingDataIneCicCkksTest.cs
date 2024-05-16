using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.INE_CIC_numbers;
using Microsoft.Research.SEAL;

namespace Tests.Identifying_data_tests.INE_CIC_numbers_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicCkksTests
{
    private Ckks _ckks = null!;
    private long _ineCicNumber;
    private Ciphertext? _ineCicNumberEncrypted;

    [GlobalSetup(Target = nameof(EncryptIneCicCkks))]
    public void SetupEncryption()
    {
        _ckks = new Ckks();
        _ineCicNumber = IneCicNumbersGenerator.GenerateIneCicNumber();
    }

    [Benchmark]
    public Ciphertext EncryptIneCicCkks() => _ckks.Encrypt(_ineCicNumber);

    [GlobalSetup(Target = nameof(DecryptIneCicCkks))]
    public void SetupDecryption()
    {
        _ckks = new Ckks();

        var generatedDate = IneCicNumbersGenerator.GenerateIneCicNumber();
        _ineCicNumberEncrypted = _ckks.Encrypt(generatedDate);
    }

    [Benchmark]
    public long DecryptIneCicCkks() => _ckks.Decrypt(_ineCicNumberEncrypted!);
}