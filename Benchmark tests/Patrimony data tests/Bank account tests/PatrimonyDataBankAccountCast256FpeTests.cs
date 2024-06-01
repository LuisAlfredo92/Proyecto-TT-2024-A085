using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using FPE_ciphers;
using Patrimony_data.Bank_accounts;
using Patrimony_data.Bank_accounts;

namespace Bank_account_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataBankAccountCast256FpeTests
{
    private Cast256Fpe _cast256Fpe = null!;
    private char[] _bankAccount = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "1234567890".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptBankAccountCast256Fpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);
        _bankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptBankAccountCast256Fpe() => _cast256Fpe.Encrypt(_bankAccount);

    [GlobalSetup(Target = nameof(DecryptBankAccountCast256Fpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _cast256Fpe = new Cast256Fpe(_key.AsSpan(), _alphabet);

        var generatedBankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
        _bankAccount = _cast256Fpe.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public char[] DecryptBankAccountCast256Fpe() => _cast256Fpe.Decrypt(_bankAccount);
}