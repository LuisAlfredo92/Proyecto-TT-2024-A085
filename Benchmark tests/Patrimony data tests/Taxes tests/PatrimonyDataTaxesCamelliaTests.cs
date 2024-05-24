using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Taxes;

namespace Taxes_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTaxesCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _taxes = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptTaxesCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _taxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTaxesCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_taxes);
    }

    [GlobalSetup(Target = nameof(DecryptTaxesCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedTaxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
        _taxes = _camellia.Encrypt(generatedTaxes);
    }

    [Benchmark]
    public byte[] DecryptTaxesCamellia() => _camellia.Decrypt(_taxes);
}