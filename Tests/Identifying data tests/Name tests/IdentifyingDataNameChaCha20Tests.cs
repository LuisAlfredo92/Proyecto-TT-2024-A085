using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Identifying_data.Names;
using System.Security.Cryptography;
using System.Text;
using Stream_ciphers;

namespace Tests.Identifying_data_tests.Name_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1000, iterationCount: 10)]
public class IdentifyingDataNameChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _name = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptNamesChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _name = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptNamesChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_name);
    }

    [GlobalSetup(Target = nameof(DecryptNamesChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedName = Encoding.UTF8.GetBytes(NamesGenerator.Generate());
        _name = _chaCha20.Encrypt(generatedName);
    }

    [Benchmark]
    public byte[] DecryptNamesChaCha20() => _chaCha20.Decrypt(_name);
}