using BenchmarkDotNet.Attributes;
using Hashes;
using Identifying_data.INE_CIC_numbers;

namespace Tests.Identifying_data_tests.INE_CIC_numbers_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicSha2Tests
{
    private byte[] _ineCicNumber = null!;

    [GlobalSetup(Target = nameof(EncryptCurpSha2))]
    public void SetupEncryption()
    {
        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public Span<byte> EncryptCurpSha2() => Sha2.Hash(_ineCicNumber);
}