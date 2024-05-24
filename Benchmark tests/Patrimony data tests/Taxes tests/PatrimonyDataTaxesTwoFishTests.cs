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
public class PatrimonyDataTaxesTwoFishTests
{
    private TwoFish _twoFish = null!;
    private byte[] _taxes = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanTwoFishBenchmark), nameof(EncryptTaxesTwoFish)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce!);

        _taxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
    }

    [Benchmark]
    public byte[] CleanTwoFishBenchmark()
    {
        _twoFish.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTaxesTwoFish()
    {
        _twoFish.Reset();
        return _twoFish.Encrypt(_taxes);
    }

    [GlobalSetup(Target = nameof(DecryptTaxesTwoFish))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _twoFish = new TwoFish(_key.AsSpan(), _nonce);

        var generatedTaxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
        _taxes = _twoFish.Encrypt(generatedTaxes);
    }

    [Benchmark]
    public byte[] DecryptTaxesTwoFish() => _twoFish.Decrypt(_taxes);
}