using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Identifying_data.Phone_numbers;

namespace Phone_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class IdentifyingDataPhoneRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _phone = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptPhoneRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _phone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
    }

    [Benchmark]
    public byte[] EncryptPhoneRsa() => _rsa.Encrypt(_phone);

    [GlobalSetup(Target = nameof(DecryptPhoneRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedPhone = BitConverter.GetBytes(PhoneNumbersGenerator.GeneratePhoneNumber());
        _phone = _rsa.Encrypt(generatedPhone);
    }

    [Benchmark]
    public byte[] DecryptPhoneRsa() => _rsa.Decrypt(_phone);
}