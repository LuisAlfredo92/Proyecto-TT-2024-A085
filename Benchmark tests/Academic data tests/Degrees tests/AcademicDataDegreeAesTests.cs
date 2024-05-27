using System.Security.Cryptography;
using Academic_data.Degrees;
using BenchmarkDotNet.Attributes;
using Aes = BlockCiphers.Aes;

namespace Degrees_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class AcademicDataDegreeAesTests
{
    private Aes _aes = null!;
    private byte[] _degree = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptDegreeAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _degree = File.ReadAllBytes(DegreeGenerator.Generate());
    }

    [Benchmark]
    public byte[] EncryptDegreeAes() => _aes.Encrypt(_degree, out _);

    [GlobalSetup(Target = nameof(DecryptDegreeAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedDegree = File.ReadAllBytes(DegreeGenerator.Generate());
        _degree = _aes.Encrypt(generatedDegree, out _tag);
    }

    [Benchmark]
    public byte[] DecryptDegreeAes() => _aes.Decrypt(_degree, _tag);
}