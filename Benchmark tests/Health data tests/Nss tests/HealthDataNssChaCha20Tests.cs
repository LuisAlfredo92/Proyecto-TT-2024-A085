using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Health_data.Nss;
using Stream_ciphers;

namespace Nss_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataNssChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _nss = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptNssChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNssChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_nss);
    }

    [GlobalSetup(Target = nameof(DecryptNssChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedNss = BitConverter.GetBytes(NssGenerator.Generate());
        _nss = _chaCha20.Encrypt(generatedNss);
    }

    [Benchmark]
    public byte[] DecryptNssChaCha20() => _chaCha20.Decrypt(_nss);
}