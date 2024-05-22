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
public class TransitAndMigratoryPassportIdCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _passportId = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptPassportIdCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPassportIdCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_passportId);
    }

    [GlobalSetup(Target = nameof(DecryptPassportIdCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _cast256.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public byte[] DecryptPassportIdCast256() => _cast256.Decrypt(_passportId);
}