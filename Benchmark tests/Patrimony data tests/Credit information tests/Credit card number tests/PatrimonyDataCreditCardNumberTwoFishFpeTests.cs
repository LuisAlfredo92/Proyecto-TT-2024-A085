using System.Globalization;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Card_number;

namespace Credit_card_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCreditCardNumberTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _creditCardNumber = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCreditCardNumberTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _creditCardNumber = CardNumberGenerator.GenerateCardNumber().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCreditCardNumberTwoFishFpe() => _twoFishFpe.Encrypt(_creditCardNumber);

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedCreditCardNumber = CardNumberGenerator.GenerateCardNumber().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _creditCardNumber = _twoFishFpe.Encrypt(generatedCreditCardNumber);
    }

    [Benchmark]
    public char[] DecryptCreditCardNumberTwoFishFpe() => _twoFishFpe.Decrypt(_creditCardNumber);
}