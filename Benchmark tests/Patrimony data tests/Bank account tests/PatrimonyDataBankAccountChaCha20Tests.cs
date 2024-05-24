using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Bank_accounts;
using Stream_ciphers;

namespace Bank_account_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataBankAccountChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _bankAccount = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptBankAccountChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _bankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptBankAccountChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_bankAccount);
    }

    [GlobalSetup(Target = nameof(DecryptBankAccountChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedBankAccount = Encoding.UTF8.GetBytes(BankAccounts.GenerateBankAccount());
        _bankAccount = _chaCha20.Encrypt(generatedBankAccount);
    }

    [Benchmark]
    public byte[] DecryptBankAccountChaCha20() => _chaCha20.Decrypt(_bankAccount);
}