using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Rfc;

namespace Rfc_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataRfcRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _rfc = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptRfcRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptRfcRsa() => _rsa.Encrypt(_rfc);

    [GlobalSetup(Target = nameof(DecryptRfcRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedRfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
        _rfc = _rsa.Encrypt(generatedRfc);
    }

    [Benchmark]
    public byte[] DecryptRfcRsa() => _rsa.Decrypt(_rfc);
}