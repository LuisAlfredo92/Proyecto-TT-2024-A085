using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Card_number;
using Aes = BlockCiphers.Aes;

namespace Credit_card_number_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCreditCardNumberAesTests
{
    private Aes _aes = null!;
    private byte[] _creditCardNumber = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptCreditCardNumberAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public byte[] EncryptCreditCardNumberAes() => _aes.Encrypt(_creditCardNumber, out _);

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedCreditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
        _creditCardNumber = _aes.Encrypt(generatedCreditCardNumber, out _tag);
    }

    [Benchmark]
    public byte[] DecryptCreditCardNumberAes() => _aes.Decrypt(_creditCardNumber, _tag);
}