using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Passport_id;
using Aes = BlockCiphers.Aes;

namespace Passport_id_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdAesTests
{
    private Aes _aes = null!;
    private byte[] _passportId = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptPassportIdAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] EncryptPassportIdAes() => _aes.Encrypt(_passportId, out _);

    [GlobalSetup(Target = nameof(DecryptPassportIdAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _aes.Encrypt(generatedPassportId, out _tag);
    }

    [Benchmark]
    public byte[] DecryptPassportIdAes() => _aes.Decrypt(_passportId, _tag);
}