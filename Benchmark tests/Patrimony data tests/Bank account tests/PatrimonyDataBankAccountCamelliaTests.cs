using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Bank_accounts;

namespace Bank_account_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataBankAccountCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _bankAccount = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptBankAccountCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _bankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptBankAccountCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_bankAccount);
    }

    [GlobalSetup(Target = nameof(DecryptBankAccountCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedBankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
        _bankAccount = _camellia.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public byte[] DecryptBankAccountCamellia() => _camellia.Decrypt(_bankAccount);
}