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
public class TransitAndMigratoryPassportIdBCryptTests
{
    private readonly BCrypt _argon2Id = new();
    private byte[] _passportId = null!;

    [GlobalSetup(Target = nameof(EncryptPassportIdBCrypt))]
    public void SetupEncryption()
    {
        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public Span<byte> EncryptPassportIdBCrypt() => _argon2Id.Hash(_passportId);
}