using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Scars;
using Stream_ciphers;

namespace Scar_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataScarChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _scar = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptScarChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _scar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptScarChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_scar);
    }

    [GlobalSetup(Target = nameof(DecryptScarChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedScar = File.ReadAllBytes(ScarsGenerator.GeneratePhoto());
        _scar = _chaCha20.Encrypt(generatedScar);
    }

    [Benchmark]
    public byte[] DecryptScarChaCha20() => _chaCha20.Decrypt(_scar);
}