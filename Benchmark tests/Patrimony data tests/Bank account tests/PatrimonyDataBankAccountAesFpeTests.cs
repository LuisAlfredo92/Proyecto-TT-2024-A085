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
public class PatrimonyDataBankAccountAesFpeTests
{
    private AesFpe _aesFpe = null!;
    private char[] _bankAccount = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "1234567890".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptBankAccountAesFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        _bankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptBankAccountAesFpe() => _aesFpe.Encrypt(_bankAccount);

    [GlobalSetup(Target = nameof(DecryptBankAccountAesFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _aesFpe = new AesFpe(_key.AsSpan(), _alphabet);

        var generatedBankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
        _bankAccount = _aesFpe.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public char[] DecryptBankAccountAesFpe() => _aesFpe.Decrypt(_bankAccount);
}