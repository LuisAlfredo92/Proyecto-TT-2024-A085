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
public class HealthDataNssCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _nss = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptNssCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _nss = BitConverter.GetBytes(NssGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNssCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_nss);
    }

    [GlobalSetup(Target = nameof(DecryptNssCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedNss = BitConverter.GetBytes(NssGenerator.Generate());
        _nss = _cast256.Encrypt(generatedNss);
    }

    [Benchmark]
    public byte[] DecryptNssCast256() => _cast256.Decrypt(_nss);
}