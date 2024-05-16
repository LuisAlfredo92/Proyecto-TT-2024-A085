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
public class IdentifyingDataIneCicPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _ineCicNumber = null!;

    [GlobalSetup(Target = nameof(EncryptCurpPbkdf2))]
    public void SetupEncryption()
    {
        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public Span<byte> EncryptCurpPbkdf2() => _argon2Id.Hash(_ineCicNumber);
}