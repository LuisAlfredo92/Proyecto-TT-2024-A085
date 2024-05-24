using System.Security.Cryptography;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Card_number;

namespace Credit_card_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCreditCardNumberRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _creditCardNumber = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptCreditCardNumberRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public byte[] EncryptCreditCardNumberRsa() => _rsa.Encrypt(_creditCardNumber);

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedCreditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
        _creditCardNumber = _rsa.Encrypt(generatedCreditCardNumber);
    }

    [Benchmark]
    public byte[] DecryptCreditCardNumberRsa() => _rsa.Decrypt(_creditCardNumber);
}