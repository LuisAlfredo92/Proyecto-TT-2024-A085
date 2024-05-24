using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Bank_accounts;
using Aes = BlockCiphers.Aes;

namespace Bank_account_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataBankAccountAesTests
{
    private Aes _aes = null!;
    private byte[] _bankAccount = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptBankAccountAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _bankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
    }

    [Benchmark]
    public byte[] EncryptBankAccountAes() => _aes.Encrypt(_bankAccount, out _);

    [GlobalSetup(Target = nameof(DecryptBankAccountAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedBankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
        _bankAccount = _aes.Encrypt(generatedBankAccount, out _tag);
    }

    [Benchmark]
    public byte[] DecryptBankAccountAes() => _aes.Decrypt(_bankAccount, _tag);
}