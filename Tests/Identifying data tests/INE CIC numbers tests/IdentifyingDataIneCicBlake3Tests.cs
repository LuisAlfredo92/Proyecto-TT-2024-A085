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
public class IdentifyingDataIneCicBlake3Tests
{
    private byte[] _ineCicNumber = null!;

    [GlobalSetup(Target = nameof(EncryptCurpBlake3))]
    public void SetupEncryption()
    {
        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public Span<byte> EncryptCurpBlake3() => Blake3.Hash(_ineCicNumber);
}