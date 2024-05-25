using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Digital_data.Email;
using Stream_ciphers;

namespace Email_tests;

[MemoryDiagnoser]
[MinColumn]
[MeanColumn]
[MedianColumn]
[MaxColumn]
[SimpleJob(launchCount: 1, iterationCount: 10)]
public class DigitalDataEmailChaCha20Tests
{
    private ChaCha20 _chaCha20 = null!;
    private byte[] _email = null!;
    private byte[]? _key;
    private byte[]? _nonce;

    [GlobalSetup(Targets = [nameof(CleanChaCha20Benchmark), nameof(EncryptEmailChaCha20)])]
    public void SetupEncryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce!);

        _email = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
    }

    [Benchmark]
    public byte[] CleanChaCha20Benchmark()
    {
        _chaCha20.Reset();
        return [];
    }

    [Benchmark]
    public byte[] EncryptEmailChaCha20()
    {
        _chaCha20.Reset();
        return _chaCha20.Encrypt(_email);
    }

    [GlobalSetup(Target = nameof(DecryptEmailChaCha20))]
    public void SetupDecryption()
    {
        _key = new byte[32];
        _nonce = new byte[12];
        RandomNumberGenerator.Fill(_key);
        RandomNumberGenerator.Fill(_nonce);
        _chaCha20 = new ChaCha20(_key.AsSpan(), _nonce);

        var generatedEmail = Encoding.UTF8.GetBytes(EmailGenerator.GenerateEmail());
        _email = _chaCha20.Encrypt(generatedEmail);
    }

    [Benchmark]
    public byte[] DecryptEmailChaCha20() => _chaCha20.Decrypt(_email);
}