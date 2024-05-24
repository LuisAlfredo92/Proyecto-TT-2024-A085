using BenchmarkDotNet.Attributes;
using Hashes;
using Patrimony_data.Card_number;

namespace Credit_card_number_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCreditCardNumberBlake3Tests
{
    private byte[] _creditCardNumber = null!;

    [GlobalSetup(Target = nameof(EncryptCreditCardNumberBlake3))]
    public void SetupEncryption()
    {
        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public Span<byte> EncryptCreditCardNumberBlake3() => Blake3.Hash(_creditCardNumber);
}