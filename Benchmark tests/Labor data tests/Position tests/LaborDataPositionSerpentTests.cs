using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using LaborData.Position;

namespace Position_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataPositionSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _position = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptPositionSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _position = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPositionSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_position);
    }

    [GlobalSetup(Target = nameof(DecryptPositionSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedPosition = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
        _position = _serpent.Encrypt(generatedPosition);
    }

    [Benchmark]
    public byte[] DecryptPositionSerpent() => _serpent.Decrypt(_position);
}