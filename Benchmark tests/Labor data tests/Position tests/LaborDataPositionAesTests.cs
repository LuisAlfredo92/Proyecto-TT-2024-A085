using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using LaborData.Position;
using Aes = BlockCiphers.Aes;

namespace Position_tests;

[MemoryDiagnoser]
[AllStatisticsColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class LaborDataPositionAesTests
{
    private Aes _aes = null!;
    private byte[] _position = null!;
    private byte[] _tag = null!;

    [GlobalSetup(Target = nameof(EncryptPositionAes))]
    public void SetupEncryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        _position = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
    }

    [Benchmark]
    public byte[] EncryptPositionAes() => _aes.Encrypt(_position, out _);

    [GlobalSetup(Target = nameof(DecryptPositionAes))]
    public void SetupDecryption()
    {
        Span<byte> key = stackalloc byte[32], nonce = stackalloc byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(key);
        RandomNumberGenerator.Fill(nonce);
        _aes = new Aes(key, nonce.ToArray());

        var generatedPosition = Encoding.UTF8.GetBytes(PositionGenerator.GeneratePosition());
        _position = _aes.Encrypt(generatedPosition, out _tag);
    }

    [Benchmark]
    public byte[] DecryptPositionAes() => _aes.Decrypt(_position, _tag);
}