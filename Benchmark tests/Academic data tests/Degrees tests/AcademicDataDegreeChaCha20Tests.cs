using System.Security.Cryptography;
using Academic_data.Degrees;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Degrees_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class AcademicDataDegreeChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _degree = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptDegreeChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _degree = File.ReadAllBytes(DegreeGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptDegreeChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_degree);
    }

    [GlobalSetup(Target = nameof(DecryptDegreeChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedDegree = File.ReadAllBytes(DegreeGenerator.Generate());
        _degree = _chaCha20.Encrypt(generatedDegree);
    }

    [Benchmark]
    public byte[] DecryptDegreeChaCha20() => _chaCha20.Decrypt(_degree);
}