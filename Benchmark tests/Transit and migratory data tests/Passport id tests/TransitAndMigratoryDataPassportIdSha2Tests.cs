using System.Text;
using BenchmarkDotNet.Attributes;
using Hashes;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdSha2Tests
{
    private byte[] _passportId = null!;

    [GlobalSetup(Target = nameof(EncryptPassportIdSha2))]
    public void SetupEncryption()
    {
        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public Span<byte> EncryptPassportIdSha2() => Sha2.Hash(_passportId);
}