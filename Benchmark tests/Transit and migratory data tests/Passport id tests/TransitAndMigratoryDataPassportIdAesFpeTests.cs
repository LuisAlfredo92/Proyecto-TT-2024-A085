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
public class TransitAndMigratoryPassportIdAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _passportId = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptPassportIdAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _passportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptPassportIdAesFpe() => _aesFpe.Encrypt(_passportId);

    [GlobalSetup(Target = nameof(DecryptPassportIdAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedPassportId = PassportIdGenerator.GeneratePassportId().ToCharArray();
        _passportId = _aesFpe.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public char[] DecryptPassportIdAesFpe() => _aesFpe.Decrypt(_passportId);
}