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
public class TransitAndMigratoryPassportIdCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _passportId = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptPassportIdCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPassportIdCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_passportId);
    }

    [GlobalSetup(Target = nameof(DecryptPassportIdCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _camellia.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public byte[] DecryptPassportIdCamellia() => _camellia.Decrypt(_passportId);
}