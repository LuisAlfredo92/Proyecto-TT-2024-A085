using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class ClassDataTypeAesTests
{
    private Aes _aes = null!;
    private byte[] _yourData = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptNamesAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _yourData = BitConverter.GetBytes(TypeGenerator.GenerateBornDate().Ticks);
    }

    [Benchmark]
    public byte[] EncryptNamesAes() => _aes.Encrypt(_yourData, out _);

    [GlobalSetup(Target = nameof(DecryptNamesAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedDate = BitConverter.GetBytes(TypeGenerator.GenerateBornDate().Ticks);
        _yourData = _aes.Encrypt(generatedDate, out _tag);
    }

    [Benchmark]
    public byte[] DecryptNamesAes() => _aes.Decrypt(_yourData, _tag);
}