using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using LaborData.Position;
using Stream_ciphers;

namespace Position_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataPositionChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _position = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptPositionChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _position = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptPositionChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_position);
    }

    [GlobalSetup(Target = nameof(DecryptPositionChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedPosition = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
        _position = _chaCha20.Encrypt(generatedPosition);
    }

    [Benchmark]
    public byte[] DecryptPositionChaCha20() => _chaCha20.Decrypt(_position);
}