using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _passportId = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptPassportIdChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPassportIdChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_passportId);
    }

    [GlobalSetup(Target = nameof(DecryptPassportIdChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _chaCha20.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public byte[] DecryptPassportIdChaCha20() => _chaCha20.Decrypt(_passportId);
}