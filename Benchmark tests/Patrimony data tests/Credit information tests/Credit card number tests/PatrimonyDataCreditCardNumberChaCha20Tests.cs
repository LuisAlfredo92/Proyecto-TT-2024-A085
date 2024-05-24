using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Card_number;
using Stream_ciphers;

namespace Credit_card_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCreditCardNumberChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _creditCardNumber = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptCreditCardNumberChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCreditCardNumberChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_creditCardNumber);
    }

    [GlobalSetup(Target = nameof(DecryptCreditCardNumberChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedCreditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
        _creditCardNumber = _chaCha20.Encrypt(generatedCreditCardNumber);
    }

    [Benchmark]
    public byte[] DecryptCreditCardNumberChaCha20() => _chaCha20.Decrypt(_creditCardNumber);
}