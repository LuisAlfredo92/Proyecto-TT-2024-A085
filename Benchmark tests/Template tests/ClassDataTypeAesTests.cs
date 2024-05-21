using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeAesTests
{
    private Aes _aes = null!;
    private byte[] _yourData = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptTypeAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _yourData = [DataGeneratorBytes];
    }

    [Benchmark]
    public byte[] EncryptTypeAes() => _aes.Encrypt(_yourData, out _);

    [GlobalSetup(Target = nameof(DecryptTypeAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedType = [DataGeneratorBytes];
        _yourData = _aes.Encrypt(generatedType, out _tag);
    }

    [Benchmark]
    public byte[] DecryptTypeAes() => _aes.Decrypt(_yourData, _tag);
}