using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Taxes;
using Stream_ciphers;

namespace Taxes_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTaxesChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _taxes = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptTaxesChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _taxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTaxesChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_taxes);
    }

    [GlobalSetup(Target = nameof(DecryptTaxesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedTaxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
        _taxes = _chaCha20.Encrypt(generatedTaxes);
    }

    [Benchmark]
    public byte[] DecryptTaxesChaCha20() => _chaCha20.Decrypt(_taxes);
}