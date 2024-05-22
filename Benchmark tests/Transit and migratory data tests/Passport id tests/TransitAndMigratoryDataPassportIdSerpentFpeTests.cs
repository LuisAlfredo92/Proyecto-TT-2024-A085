using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _passportId = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPassportIdSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _passportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPassportIdSerpentFpe() => _serpentFpe.Encrypt(_passportId);

    [GlobalSetup(Target = nameof(DecryptPassportIdSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedPassportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
        _passportId = _serpentFpe.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public char[] DecryptPassportIdSerpentFpe() => _serpentFpe.Decrypt(_passportId);
}