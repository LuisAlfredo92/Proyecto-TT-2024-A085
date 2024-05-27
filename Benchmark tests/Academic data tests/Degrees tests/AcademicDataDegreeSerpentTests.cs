using System.Security.Cryptography;
using Academic_data.Degrees;
using BenchmarkDotNet.Attributes;
using BlockCiphers;

namespace Degrees_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class AcademicDataDegreeSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _degree = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptDegreeSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _degree = File.ReadAllBytes(DegreeGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptDegreeSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_degree);
    }

    [GlobalSetup(Target = nameof(DecryptDegreeSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedDegree = File.ReadAllBytes(DegreeGenerator.Generate());
        _degree = _serpent.Encrypt(generatedDegree);
    }

    [Benchmark]
    public byte[] DecryptDegreeSerpent() => _serpent.Decrypt(_degree);
}