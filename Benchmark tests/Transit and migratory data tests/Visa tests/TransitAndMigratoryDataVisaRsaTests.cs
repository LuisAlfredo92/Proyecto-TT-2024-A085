using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Visa;

namespace Visa_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVisaRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _visa = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptVisaRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _visa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
    }

    [Benchmark]
    public byte[] EncryptVisaRsa() => _rsa.Encrypt(_visa);

    [GlobalSetup(Target = nameof(DecryptVisaRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedVisa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
        _visa = _rsa.Encrypt(generatedVisa);
    }

    [Benchmark]
    public byte[] DecryptVisaRsa() => _rsa.Decrypt(_visa);
}