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
public class PatrimonyDataTaxesCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _taxes = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptTaxesCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _taxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTaxesCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_taxes);
    }

    [GlobalSetup(Target = nameof(DecryptTaxesCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedTaxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
        _taxes = _cast256.Encrypt(generatedTaxes);
    }

    [Benchmark]
    public byte[] DecryptTaxesCast256() => _cast256.Decrypt(_taxes);
}