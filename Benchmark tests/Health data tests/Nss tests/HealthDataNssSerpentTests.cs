using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Health_data.Nss;

namespace Nss_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class HealthDataNssSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _nss = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptNssSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNssSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_nss);
    }

    [GlobalSetup(Target = nameof(DecryptNssSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedNss = BitConverter.GetBytes(NssGenerator.Generate());
        _nss = _serpent.Encrypt(generatedNss);
    }

    [Benchmark]
    public byte[] DecryptNssSerpent() => _serpent.Decrypt(_nss);
}