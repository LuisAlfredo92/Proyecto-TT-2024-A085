using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.INE_CIC_numbers;

namespace Tests.Identifying_data_tests.INE_CIC_numbers_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataIneCicRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _ineCicNumber = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptIneCicRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _ineCicNumber = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
    }

    [Benchmark]
    public byte[] EncryptIneCicRsa() => _rsa.Encrypt(_ineCicNumber);

    [GlobalSetup(Target = nameof(DecryptIneCicRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedDate = BitConverter.GetBytes(IneCicNumbersGenerator.GenerateIneCicNumber());
        _ineCicNumber = _rsa.Encrypt(generatedDate);
    }

    [Benchmark]
    public byte[] DecryptIneCicRsa() => _rsa.Decrypt(_ineCicNumber);
}