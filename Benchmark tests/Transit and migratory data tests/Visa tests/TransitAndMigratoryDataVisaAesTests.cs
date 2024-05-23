using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Visa;
using Aes = BlockCiphers.Aes;

namespace Visa_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataVisaAesTests
{
    private Aes _aes = null!;
    private byte[] _visa = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptVisaAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _visa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
    }

    [Benchmark]
    public byte[] EncryptVisaAes() => _aes.Encrypt(_visa, out _);

    [GlobalSetup(Target = nameof(DecryptVisaAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedVisa = Encoding.UTF8.GetBytes(VisaGenerator.GenerateVisaType());
        _visa = _aes.Encrypt(generatedVisa, out _tag);
    }

    [Benchmark]
    public byte[] DecryptVisaAes() => _aes.Decrypt(_visa, _tag);
}