using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Taxes;
using Aes = BlockCiphers.Aes;

namespace Taxes_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataTaxesAesTests
{
    private Aes _aes = null!;
    private byte[] _taxes = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptTaxesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _taxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
    }

    [Benchmark]
    public byte[] EncryptTaxesAes() => _aes.Encrypt(_taxes, out _);

    [GlobalSetup(Target = nameof(DecryptTaxesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedTaxes = Encoding.UTF8.GetBytes(TaxesGenerator.GenerateTaxes());
        _taxes = _aes.Encrypt(generatedTaxes, out _tag);
    }

    [Benchmark]
    public byte[] DecryptTaxesAes() => _aes.Decrypt(_taxes, _tag);
}