using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Cvv;
using Stream_ciphers;

namespace Cvv_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataCvvChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _cvv = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptCvvChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _cvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptCvvChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_cvv);
    }

    [GlobalSetup(Target = nameof(DecryptCvvChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedCvv = BitConverter.GetBytes(CvvGenerator.GenerateCvv());
        _cvv = _chaCha20.Encrypt(generatedCvv);
    }

    [Benchmark]
    public byte[] DecryptCvvChaCha20() => _chaCha20.Decrypt(_cvv);
}