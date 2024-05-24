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
public class PatrimonyDataCvvCast256Tests
{
    private Cast256 _cast256 = null!;
    private byte[] _cvv = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanCast256Benchmark), nameof(EncryptCvvCast256)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce!);

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public byte[] CleanCast256Benchmark()
    {
        _cast256.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCvvCast256()
    {
        _cast256.Reset();
        return _cast256.Encrypt(_cvv);
    }

    [GlobalSetup(Target = nameof(DecryptCvvCast256))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _cast256 = new Cast256(_key.AsSpan(), _nonce);

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvv = _cast256.Encrypt(generatedCvv);
    }

    [Benchmark]
    public byte[] DecryptCvvCast256() => _cast256.Decrypt(_cvv);
}