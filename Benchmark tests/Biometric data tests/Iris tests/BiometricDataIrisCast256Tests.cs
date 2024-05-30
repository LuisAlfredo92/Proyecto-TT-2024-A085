using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Biometric_data.Iris;
using BlockCiphers;

namespace Iris_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class BiometricDataIrisCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _iris = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptIrisCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _iris = File.ReadAllBytes(IrisGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptIrisCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_iris);
    }

    [GlobalSetup(Target = nameof(DecryptIrisCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedIris = File.ReadAllBytes(IrisGenerator.Generate());
        _iris = _cast256.Encrypt(generatedIris);
    }

    [Benchmark]
    public byte[] DecryptIrisCast256() => _cast256.Decrypt(_iris);
}