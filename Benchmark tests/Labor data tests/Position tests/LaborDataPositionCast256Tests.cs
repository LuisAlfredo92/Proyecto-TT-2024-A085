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
public class LaborDataPositionCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _position = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptPositionCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _position = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPositionCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_position);
    }

    [GlobalSetup(Target = nameof(DecryptPositionCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedPosition = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
        _position = _cast256.Encrypt(generatedPosition);
    }

    [Benchmark]
    public byte[] DecryptPositionCast256() => _cast256.Decrypt(_position);
}