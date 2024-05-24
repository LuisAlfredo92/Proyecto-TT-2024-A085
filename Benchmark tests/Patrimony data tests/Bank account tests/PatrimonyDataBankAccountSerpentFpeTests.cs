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
public class PatrimonyDataBankAccountSerpentFpeTests
{
    private SerpentFpe _serpentFpe = null!;
    private char[] _bankAccount = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "1234567890".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptBankAccountSerpentFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);
        _bankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptBankAccountSerpentFpe() => _serpentFpe.Encrypt(_bankAccount);

    [GlobalSetup(Target = nameof(DecryptBankAccountSerpentFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _serpentFpe = new SerpentFpe(_key.AsSpan(), _alphabet);

        var generatedBankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
        _bankAccount = _serpentFpe.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public char[] DecryptBankAccountSerpentFpe() => _serpentFpe.Decrypt(_bankAccount);
}