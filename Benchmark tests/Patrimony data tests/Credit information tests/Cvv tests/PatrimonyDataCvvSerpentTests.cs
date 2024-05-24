using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Patrimony_data.Cvv;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _cvv = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptCvvSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCvvSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_cvv);
    }

    [GlobalSetup(Target = nameof(DecryptCvvSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvv = _serpent.Encrypt(generatedCvv);
    }

    [Benchmark]
    public byte[] DecryptCvvSerpent() => _serpent.Decrypt(_cvv);
}