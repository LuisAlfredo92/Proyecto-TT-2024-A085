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
public class PatrimonyDataCreditCardNumberArgon2IdTests
{
    private readonly Argon2Id _argon2Id = new();
    private byte[] _creditCardNumber = null!;

    [GlobalSetup(Target = nameof(EncryptCreditCardNumberArgon2Id))]
    public void SetupEncryption()
    {
        _creditCardNumber = BitConverter.GetBytes(CardNumberGenerator.GenerateCardNumber());
    }

    [Benchmark]
    public Span<byte> EncryptCreditCardNumberArgon2Id() => _argon2Id.Hash(_creditCardNumber);
}