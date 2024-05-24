using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Afore;
using Aes = BlockCiphers.Aes;

namespace Afore_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeAesTests
{
    private Aes _aes = null!;
    private byte[] _afore = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptAforeAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _afore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
    }

    [Benchmark]
    public byte[] EncryptAforeAes() => _aes.Encrypt(_afore, out _);

    [GlobalSetup(Target = nameof(DecryptAforeAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedAfore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
        _afore = _aes.Encrypt(generatedAfore, out _tag);
    }

    [Benchmark]
    public byte[] DecryptAforeAes() => _aes.Decrypt(_afore, _tag);
}