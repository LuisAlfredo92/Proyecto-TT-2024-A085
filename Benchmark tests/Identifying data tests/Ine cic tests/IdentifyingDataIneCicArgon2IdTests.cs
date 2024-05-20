using BenchmarkDotNet.Attributes;
using Hashes;
using Identifying_data.INE_CIC_numbers;

namespace Ine_cic_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _ineCicNumber = null!;

    [GlobalSetup(Target = nameof(EncryptIneCicArgon2Id))]
    public void SetupEncryption()
    {
        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public Span<byte> EncryptIneCicArgon2Id() => _argon2Id.Hash(_ineCicNumber);
}