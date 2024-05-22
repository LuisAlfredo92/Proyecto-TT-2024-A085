using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Passport_id;

namespace Passport_id_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryPassportIdRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _passportId = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptPassportIdRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _passportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
    }

    [Benchmark]
    public byte[] EncryptPassportIdRsa() => _rsa.Encrypt(_passportId);

    [GlobalSetup(Target = nameof(DecryptPassportIdRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedPassportId = Encoding.UTF8.GetBytes(PassportIdGenerator.GeneratePassportId());
        _passportId = _rsa.Encrypt(generatedPassportId);
    }

    [Benchmark]
    public byte[] DecryptPassportIdRsa() => _rsa.Decrypt(_passportId);
}