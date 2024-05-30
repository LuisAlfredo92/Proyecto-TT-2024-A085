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
public class BiometricDataIrisSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _iris = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptIrisSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _iris = File.ReadAllBytes(IrisGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptIrisSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_iris);
    }

    [GlobalSetup(Target = nameof(DecryptIrisSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedIris = File.ReadAllBytes(IrisGenerator.Generate());
        _iris = _serpent.Encrypt(generatedIris);
    }

    [Benchmark]
    public byte[] DecryptIrisSerpent() => _serpent.Decrypt(_iris);
}