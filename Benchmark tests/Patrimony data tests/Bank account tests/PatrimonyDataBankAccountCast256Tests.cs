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
public class PatrimonyDataBankAccountCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _bankAccount = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptBankAccountCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _bankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptBankAccountCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_bankAccount);
    }

    [GlobalSetup(Target = nameof(DecryptBankAccountCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedBankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
        _bankAccount = _cast256.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public byte[] DecryptBankAccountCast256() => _cast256.Decrypt(_bankAccount);
}