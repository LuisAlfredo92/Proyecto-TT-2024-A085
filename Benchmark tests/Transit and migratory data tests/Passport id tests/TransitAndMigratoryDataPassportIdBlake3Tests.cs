using System.Text;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdBlake3Tests
{
    private byte[] _passportId = null!;

    [GlobalSetup(Target = nameof(EncryptPassportIdBlake3))]
    public void SetupEncryption()
    {
        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public Span<byte> EncryptPassportIdBlake3() => Blake3.Hash(_passportId);
}