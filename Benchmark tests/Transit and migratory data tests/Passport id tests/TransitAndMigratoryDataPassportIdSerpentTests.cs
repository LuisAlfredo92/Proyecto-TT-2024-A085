using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _passportId = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptPassportIdSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPassportIdSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_passportId);
    }

    [GlobalSetup(Target = nameof(DecryptPassportIdSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _serpent.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public byte[] DecryptPassportIdSerpent() => _serpent.Decrypt(_passportId);
}