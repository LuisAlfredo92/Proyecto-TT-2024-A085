using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Ideological_data.Religion;

namespace Religion_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataReligionSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _religion = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptReligionSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _religion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReligionSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_religion);
    }

    [GlobalSetup(Target = nameof(DecryptReligionSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedReligion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
        _religion = _serpent.Encrypt(generatedReligion);
    }

    [Benchmark]
    public byte[] DecryptReligionSerpent() => _serpent.Decrypt(_religion);
}