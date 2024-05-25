using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BlockCiphers;
using Digital_data.Email;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 100, iterationCount: 10)]
public class DigitalDataEmailSerpentTests
{
    private Serpent _serpent = null!;
    private byte[] _email = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanSerpentBenchmark), nameof(EncryptEmailSerpent)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce!);

        _email = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
    }

    [Benchmark]
    public byte[] CleanSerpentBenchmark()
    {
        _serpent.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEmailSerpent()
    {
        _serpent.Reset();
        return _serpent.Encrypt(_email);
    }

    [GlobalSetup(Target = nameof(DecryptEmailSerpent))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[8];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _serpent = new Serpent(_key.AsSpan(), _nonce);

        var generatedEmail = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
        _email = _serpent.Encrypt(generatedEmail);
    }

    [Benchmark]
    public byte[] DecryptEmailSerpent() => _serpent.Decrypt(_email);
}