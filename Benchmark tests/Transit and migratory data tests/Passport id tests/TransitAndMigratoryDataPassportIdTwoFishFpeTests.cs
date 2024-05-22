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
public class TransitAndMigratoryPassportIdTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _passportId = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPassportIdTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _passportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPassportIdTwoFishFpe() => _twoFishFpe.Encrypt(_passportId);

    [GlobalSetup(Target = nameof(DecryptPassportIdTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedPassportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
        _passportId = _twoFishFpe.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public char[] DecryptPassportIdTwoFishFpe() => _twoFishFpe.Decrypt(_passportId);
}