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
public class TransitAndMigratoryPassportIdPbkdf2Tests
{
    private readonly Pbkdf2 _argon2Id = new();
    private byte[] _passportId = null!;

    [GlobalSetup(Target = nameof(EncryptPassportIdPbkdf2))]
    public void SetupEncryption()
    {
        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public Span<byte> EncryptPassportIdPbkdf2() => _argon2Id.Hash(_passportId);
}