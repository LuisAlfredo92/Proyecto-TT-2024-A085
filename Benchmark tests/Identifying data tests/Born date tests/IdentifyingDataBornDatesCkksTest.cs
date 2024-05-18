using BenchmarkDotNet.Attributes;
using Homomorphic_ciphers;
using Identifying_data.Born_dates;
using Microsoft.Research.SEAL;

namespace Born_date_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataBornDatesCkksTests
{
    private Ckks _ckks = null!;
    private long _bornDate;
    private Ciphertext? _bornDateEncrypted;

    [GlobalSetup(Target = nameof(EncryptBornDatesCkks))]
    public void SetupEncryption()
    {
        _ckks = new Ckks();
        _bornDate = BornDatesGenerator.GenerateBornDate().Ticks;
    }

    [Benchmark]
    public Ciphertext EncryptBornDatesCkks() => _ckks.Encrypt(_bornDate);

    [GlobalSetup(Target = nameof(DecryptBornDatesCkks))]
    public void SetupDecryption()
    {
        _ckks = new Ckks();

        var generatedDate = BornDatesGenerator.GenerateBornDate().Ticks;
        _bornDateEncrypted = _ckks.Encrypt(generatedDate);
    }

    [Benchmark]
    public long DecryptBornDatesCkks() => _ckks.Decrypt(_bornDateEncrypted!);
}