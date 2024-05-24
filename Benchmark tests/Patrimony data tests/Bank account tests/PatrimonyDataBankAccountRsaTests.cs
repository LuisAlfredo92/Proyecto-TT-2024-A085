using System.Security.Cryptography;
using System.Text;
using Asymmetric_ciphers;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Bank_accounts;

namespace Bank_account_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataBankAccountRsaTests
{
    private Rsa _rsa = null!;
    private byte[] _bankAccount = null!;
    private byte[]? _key;
    private RSACryptoServiceProvider? _provider;

    [GlobalSetup(Target = nameof(EncryptBankAccountRsa))]
    public void SetupEncryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        _bankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
    }

    [Benchmark]
    public byte[] EncryptBankAccountRsa() => _rsa.Encrypt(_bankAccount);

    [GlobalSetup(Target = nameof(DecryptBankAccountRsa))]
    public void SetupDecryption()
    {
        _provider = new RSACryptoServiceProvider(4096);
        _key = _provider.ExportRSAPrivateKey();
        _rsa = new Rsa(_key);

        var generatedBankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
        _bankAccount = _rsa.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public byte[] DecryptBankAccountRsa() => _rsa.Decrypt(_bankAccount);
}