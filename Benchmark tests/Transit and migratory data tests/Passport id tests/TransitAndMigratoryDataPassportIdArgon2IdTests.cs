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
public class TransitAndMigratoryPassportIdArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _passportId = null!;

    [GlobalSetup(Target = nameof(EncryptPassportIdArgon2Id))]
    public void SetupEncryption()
    {
        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public Span<byte> EncryptPassportIdArgon2Id() => _argon2Id.Hash(_passportId);
}