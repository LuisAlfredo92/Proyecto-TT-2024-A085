using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Bank_accounts;

namespace Bank_account_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataBankAccountTwoFishFpeTests
{
    private TwoFishFpe _twoFishFpe = null!;
    private char[] _bankAccount = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "1234567890".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptBankAccountTwoFishFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);
        _bankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptBankAccountTwoFishFpe() => _twoFishFpe.Encrypt(_bankAccount);

    [GlobalSetup(Target = nameof(DecryptBankAccountTwoFishFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _twoFishFpe = new TwoFishFpe(_key.AsSpan(), _alphabet);

        var generatedBankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
        _bankAccount = _twoFishFpe.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public char[] DecryptBankAccountTwoFishFpe() => _twoFishFpe.Decrypt(_bankAccount);
}