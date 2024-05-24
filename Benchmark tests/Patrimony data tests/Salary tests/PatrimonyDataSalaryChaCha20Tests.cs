using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Salary;
using Stream_ciphers;

namespace Salary_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataSalaryChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _salary = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptSalaryChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _salary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptSalaryChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_salary);
    }

    [GlobalSetup(Target = nameof(DecryptSalaryChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedSalary = BitConverter.GetBytes(SalaryGenerator.GenerateSalary());
        _salary = _chaCha20.Encrypt(generatedSalary);
    }

    [Benchmark]
    public byte[] DecryptSalaryChaCha20() => _chaCha20.Decrypt(_salary);
}