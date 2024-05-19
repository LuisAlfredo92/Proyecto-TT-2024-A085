using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Identifying_data.Rfc;
using Stream_ciphers;

namespace Rfc_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdentifyingDataRfcChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _rfc = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptRfcChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _rfc = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptRfcChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_rfc);
    }

    [GlobalSetup(Target = nameof(DecryptRfcChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedName = Encoding.UTF8.GetBytes(RfcGenerator.Generate());
        _rfc = _chaCha20.Encrypt(generatedName);
    }

    [Benchmark]
    public byte[] DecryptRfcChaCha20() => _chaCha20.Decrypt(_rfc);
}