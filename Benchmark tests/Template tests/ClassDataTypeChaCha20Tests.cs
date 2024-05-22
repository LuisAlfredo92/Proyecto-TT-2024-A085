using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Stream_ciphers;

namespace Tests.Template_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class ClassDataTypeChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _yourData = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptTypeChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _yourData = [DataGeneratorBytes];
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptTypeChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_yourData);
    }

    [GlobalSetup(Target = nameof(DecryptTypeChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedType = [DataGeneratorBytes];
        _yourData = _chaCha20.Encrypt(generatedType);
    }

    [Benchmark]
    public byte[] DecryptTypeChaCha20() => _chaCha20.Decrypt(_yourData);
}