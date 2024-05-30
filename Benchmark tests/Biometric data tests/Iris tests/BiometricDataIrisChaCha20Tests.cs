using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Iris;
using Stream_ciphers;

namespace Iris_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataIrisChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _iris = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptIrisChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _iris = File.ReadAllBytes(IrisGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptIrisChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_iris);
    }

    [GlobalSetup(Target = nameof(DecryptIrisChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedIris = File.ReadAllBytes(IrisGenerator.Generate());
        _iris = _chaCha20.Encrypt(generatedIris);
    }

    [Benchmark]
    public byte[] DecryptIrisChaCha20() => _chaCha20.Decrypt(_iris);
}