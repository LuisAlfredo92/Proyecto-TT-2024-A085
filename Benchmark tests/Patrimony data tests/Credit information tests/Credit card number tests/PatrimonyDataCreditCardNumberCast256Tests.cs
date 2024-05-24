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
public class PatrimonyDataCreditCardNumberCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _creditCardNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptCreditCardNumberCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCreditCardNumberCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_creditCardNumber);
    }

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedCreditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
        _creditCardNumber = _cast256.Encrypt(generatedCreditCardNumber);
    }

    [Benchmark]
    public byte[] DecryptCreditCardNumberCast256() => _cast256.Decrypt(_creditCardNumber);
}