using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Names;

namespace Name_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataNameSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _name = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptNamesSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _name = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_name);
    }

    [GlobalSetup(Target = nameof(DecryptNamesSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedName = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
        _name = _serpent.Encrypt(generatedName);
    }

    [Benchmark]
    public byte[] DecryptNamesSerpent() => _serpent.Decrypt(_name);
}