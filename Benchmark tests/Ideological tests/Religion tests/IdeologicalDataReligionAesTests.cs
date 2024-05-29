using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Ideological_data.Religion;
using Aes = BlockCiphers.Aes;

namespace Religion_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataReligionAesTests
{
    private Aes _aes = null!;
    private byte[] _religion = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptReligionAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _religion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptReligionAes() => _aes.Encrypt(_religion, out _);

    [GlobalSetup(Target = nameof(DecryptReligionAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedReligion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
        _religion = _aes.Encrypt(generatedReligion, out _tag);
    }

    [Benchmark]
    public byte[] DecryptReligionAes() => _aes.Decrypt(_religion, _tag);
}