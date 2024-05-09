using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Curps;

namespace Tests.Identifying_data_tests.CURP_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataCurpRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _curp = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptCurpRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _curp = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptCurpRsa() => _rsa.Encrypt(_curp);

    [GlobalSetup(Target = nameof(DecryptCurpRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedDate = Encoding.UTF8.GetBytes(CurpsGenerator.Generate());
        _curp = _rsa.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptCurpRsa() => _rsa.Decrypt(_curp);
}