using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Digital_data.Usernames;

namespace Username_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataUsernameSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _username = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptUsernameSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _username = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptUsernameSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_username);
    }

    [GlobalSetup(Target = nameof(DecryptUsernameSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedUsername = Encoding.UTF8.GetBytes(UsernamesGenerator.GenerateUsername());
        _username = _serpent.Encrypt(generatedUsername);
    }

    [Benchmark]
    public byte[] DecryptUsernameSerpent() => _serpent.Decrypt(_username);
}