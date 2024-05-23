using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Transit_and_migratory_data.Niv;
using Aes = BlockCiphers.Aes;

namespace Niv_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class TransitAndMigratoryDataNivAesTests
{
    private Aes _aes = null!;
    private byte[] _niv = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptNivAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _niv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
    }

    [Benchmark]
    public byte[] EncryptNivAes() => _aes.Encrypt(_niv, out _);

    [GlobalSetup(Target = nameof(DecryptNivAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedNiv = Encoding.UTF8.GetBytes(NivGenerator.GenerateNiv());
        _niv = _aes.Encrypt(generatedNiv, out _tag);
    }

    [Benchmark]
    public byte[] DecryptNivAes() => _aes.Decrypt(_niv, _tag);
}