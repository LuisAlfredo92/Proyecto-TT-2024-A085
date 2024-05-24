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
public class PatrimonyDataBankAccountCamelliaFpeTests
{
    private CamelliaFpe _camelliaFpe = null!;
    private char[] _bankAccount = null!;
    private byte[]? _key;
    private readonly char[] _alphabet = "1234567890".ToCharArray();

    [GlobalSetup(Target = nameof(EncryptBankAccountCamelliaFpe))]
    public void SetupEncryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);
        _bankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
    }

    [Benchmark]
    public char[] EncryptBankAccountCamelliaFpe() => _camelliaFpe.Encrypt(_bankAccount);

    [GlobalSetup(Target = nameof(DecryptBankAccountCamelliaFpe))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        RandomNumberGenerator.Fill(_key);
        _camelliaFpe = new CamelliaFpe(_key.AsSpan(), _alphabet);

        var generatedBankAccount = BankAccounts.GenerateBankAccount().ToCharArray();
        _bankAccount = _camelliaFpe.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public char[] DecryptBankAccountCamelliaFpe() => _camelliaFpe.Decrypt(_bankAccount);
}