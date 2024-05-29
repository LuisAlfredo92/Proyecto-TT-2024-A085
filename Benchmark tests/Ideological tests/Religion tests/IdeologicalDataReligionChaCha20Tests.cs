using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Ideological_data.Religion;
using Stream_ciphers;

namespace Religion_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class IdeologicalDataReligionChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _religion = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptReligionChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _religion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptReligionChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_religion);
    }

    [GlobalSetup(Target = nameof(DecryptReligionChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedReligion = Encoding.UTF8.GetBytes(ReligionGenerator.Generate());
        _religion = _chaCha20.Encrypt(generatedReligion);
    }

    [Benchmark]
    public byte[] DecryptReligionChaCha20() => _chaCha20.Decrypt(_religion);
}