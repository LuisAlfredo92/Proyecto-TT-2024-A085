using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Card_number;

namespace Credit_card_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCreditCardNumberTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _creditCardNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptCreditCardNumberTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCreditCardNumberTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_creditCardNumber);
    }

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedCreditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
        _creditCardNumber = _twoFish.Encrypt(generatedCreditCardNumber);
    }

    [Benchmark]
    public byte[] DecryptCreditCardNumberTwoFish() => _twoFish.Decrypt(_creditCardNumber);
}