using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Digital_data.Usernames;
using Stream_ciphers;

namespace Username_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataUsernameChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _username = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptUsernameChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _username = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUsernameChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_username);
    }

    [GlobalSetup(Target = nameof(DecryptUsernameChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedUsername = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
        _username = _chaCha20.Encrypt(generatedUsername);
    }

    [Benchmark]
    public byte[] DecryptUsernameChaCha20() => _chaCha20.Decrypt(_username);
}