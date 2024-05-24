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
public class PatrimonyDataCreditCardNumberCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _creditCardNumber = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "0123456789".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptCreditCardNumberCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _creditCardNumber = CardNumberGenerator.GenerateCardNumber().ToString(CultureInfo.InvariantCulture).ToCharArray();
    }

    [Benchmark]
    public char[] EncryptCreditCardNumberCast256Fpe() => _cast256Fpe.Encrypt(_creditCardNumber);

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedCreditCardNumber = CardNumberGenerator.GenerateCardNumber().ToString(CultureInfo.InvariantCulture).ToCharArray();
        _creditCardNumber = _cast256Fpe.Encrypt(generatedCreditCardNumber);
    }

    [Benchmark]
    public char[] DecryptCreditCardNumberCast256Fpe() => _cast256Fpe.Decrypt(_creditCardNumber);
}