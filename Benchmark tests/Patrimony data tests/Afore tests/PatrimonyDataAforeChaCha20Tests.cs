using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Patrimony_data.Afore;
using Stream_ciphers;

namespace Afore_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class PatrimonyDataAforeChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _afore = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptAforeChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _afore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptAforeChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_afore);
    }

    [GlobalSetup(Target = nameof(DecryptAforeChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedAfore = Encoding.UTF8.GetBytes(AforeGenerator.GenerateAforeName());
        _afore = _chaCha20.Encrypt(generatedAfore);
    }

    [Benchmark]
    public byte[] DecryptAforeChaCha20() => _chaCha20.Decrypt(_afore);
}