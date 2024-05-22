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
public class LaborDataPositionCamelliaTests
{
    private Camellia _camellia = null!;
    private byte[] _position = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCamelliaBenchmark), nameof(EncryptPositionCamellia)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce!);

        _position = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
    }

    [Benchmark]
    public byte[] CleanCamelliaBenchmark()
    {
        _camellia.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPositionCamellia()
    {
        _camellia.Reset();
        return _camellia.Encrypt(_position);
    }

    [GlobalSetup(Target = nameof(DecryptPositionCamellia))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _camellia = new Camellia(_key.AsSpan(), _nonce);

        var generatedPosition = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
        _position = _camellia.Encrypt(generatedPosition);
    }

    [Benchmark]
    public byte[] DecryptPositionCamellia() => _camellia.Decrypt(_position);
}