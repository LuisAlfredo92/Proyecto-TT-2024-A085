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
public class TransitAndMigratoryPassportIdTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _passportId = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptPassportIdTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPassportIdTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_passportId);
    }

    [GlobalSetup(Target = nameof(DecryptPassportIdTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _twoFish.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public byte[] DecryptPassportIdTwoFish() => _twoFish.Decrypt(_passportId);
}